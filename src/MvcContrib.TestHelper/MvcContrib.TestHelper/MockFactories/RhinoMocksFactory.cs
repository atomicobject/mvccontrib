using System;
using System.Reflection;
using System.Linq;

namespace MvcContrib.TestHelper.MockFactories
{
	/// <summary>
	/// Creates mock objects using Rhino Mocks dynamically. 
	/// </summary>
	internal class RhinoMocksFactory : IMockFactory
	{
		internal static Assembly RhinoMocks;

		private static readonly object _mocks;
		private static readonly Exception _loadException;
		private static readonly MethodInfo _dynamicMockOpen;

		/// <summary>
		/// Grabs references to Rhino Mocks types using reflection.
		/// </summary>
		static RhinoMocksFactory()
		{
			try
			{
				RhinoMocks = Assembly.Load("Rhino.Mocks");
				var repositoryType = RhinoMocks.GetType("Rhino.Mocks.MockRepository");
				_dynamicMockOpen = repositoryType.GetMethods().First(m => m.Name == "DynamicMock" && m.IsGenericMethod);
				_mocks = Activator.CreateInstance(repositoryType);
			}
			catch(Exception ex)
			{
				_loadException = ex;
			}
		}

		/// <summary>
		/// Initializes the factory.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if Rhino Mocks can't be loaded.</exception>
		public RhinoMocksFactory()
		{			
			if (_mocks == null)
			{
				throw new InvalidOperationException("Unable to create a proxy for RhinoMocks.", _loadException);
			}
		}

		/// <summary>
		/// Creates a dynamic mock for the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IMockProxy<T> DynamicMock<T>() where T : class
		{
			var dynamicMock = _dynamicMockOpen.MakeGenericMethod(typeof(T));
			return new RhinoMocksProxy<T>((T)dynamicMock.Invoke(_mocks, new object[] { new object[0]}), _mocks);
		}
	}
}
