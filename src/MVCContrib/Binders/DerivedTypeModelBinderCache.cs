using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace MvcContrib.Binders
{
	/// <summary>
	/// This cache is used to both improve performance of the derived type model binder
	/// on cases where a binding type has already been identified.
	/// </summary>
	public static class DerivedTypeModelBinderCache
	{
		private static readonly ThreadSafeDictionary<Type, IEnumerable<Type>> _typeCache =
			new ThreadSafeDictionary<Type, IEnumerable<Type>>();

		private static readonly ConcurrentDictionary<string, Type> _hashToTypeDictionary =
			new ConcurrentDictionary<string, Type>();

		private static readonly ConcurrentDictionary<Type, string> _typeToHashDictionary =
			new ConcurrentDictionary<Type, string>();
		
		private static string _typeStampFieldName = "_xTypeStampx_";

		public static string TypeStampFieldName
		{
			get { return _typeStampFieldName; } 
			set { _typeStampFieldName = value; }
		}

		/// <summary>
		/// Registers the attached set of derived types by the indicated base type
		/// </summary>
		/// <param name="baseType">base type that will be encountered by the binder where an alternate value should be used</param>
		/// <param name="derivedTypes">an enumerable set of types to be considered for binding</param>
		public static bool RegisterDerivedTypes(Type baseType, IEnumerable<Type> derivedTypes)
		{
			try
			{
				// register the types based on the base type
				_typeCache.Add(baseType, derivedTypes);
			}
			catch (ArgumentException)
			{
				return false;
			}

			foreach (var item in derivedTypes)
			{
				// this step is needed to make sure the closure behaves properly
				var currentItem = item;
				var encryptedName = EncryptStringToBase64(currentItem.FullName);

				_hashToTypeDictionary.AddOrUpdate(encryptedName, name => currentItem,
				                               (name, itemValue) => currentItem);

				_typeToHashDictionary.AddOrUpdate(currentItem, type => encryptedName, (type, name) => encryptedName);
			}
			// register the base type with the derived type modelbinder for binding purposes
			ModelBinders.Binders.Add(baseType, new DerivedTypeModelBinder());

			return true;

		}

		private static readonly byte[] _originalEncryptionSalt = new byte[]
                                                          {
                                                              0xc5, 0x10, 0x53, 0xe3, 0xc4, 0x17, 0x47, 0xc9,
                                                              0x85, 0x5d, 0xf1, 0x62, 0x73, 0x94, 0x12, 0x9e
                                                          };

		private static byte[] _activeEncryptionSalt = _originalEncryptionSalt;


		public static void SetTypeStampSaltValue(Guid guid)
		{
			_activeEncryptionSalt = guid.ToByteArray();
		}

		private static string EncryptStringToBase64(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			var passwordData = Encoding.UTF8.GetBytes(value);

			var hashAlgorithm = new HMACSHA256(_activeEncryptionSalt);

			var hashBytes = hashAlgorithm.ComputeHash(passwordData);

			return Convert.ToBase64String(hashBytes);
		}

		public static Type GetDerivedType(string typeValue)
		{
			Type type;
			return _hashToTypeDictionary.TryGetValue(typeValue, out type) ? type : null;
		}

		/// <summary>
		/// removes all items from the cache
		/// </summary>
		public static void Reset()
		{
			// first, remove all type registrations in Mvc's binder registry
			foreach (var type in _typeCache)
				ModelBinders.Binders.Remove(type.Key);

			_hashToTypeDictionary.Clear();
			_typeToHashDictionary.Clear();

			// clear the cache
			_typeCache.Clear();

			// reset salt value
			_activeEncryptionSalt = _originalEncryptionSalt;
		}

		/// <summary>
		/// Gets a hashed name for the given type and also checks to see if the type has been registered.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static string GetTypeName(Type type)
		{
			try
			{
				return _typeToHashDictionary[type];
			}
			catch( KeyNotFoundException keyNotFoundException)
			{
				throw new KeyNotFoundException(
					string.Format("Type {0} is not registered with the DerivedTypeModelBinder", type.Name), keyNotFoundException);
			}
		}
	}
}


