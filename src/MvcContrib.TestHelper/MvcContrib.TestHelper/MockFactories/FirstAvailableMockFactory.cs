using System;
using System.Reflection;

namespace MvcContrib.TestHelper.MockFactories
{
	/// <summary>
	/// A mock factory that uses the first available mocking framework
	/// to create mock objects.
	/// </summary>
	/// <remarks>
	/// This factory will attempt to use <see cref="RhinoMocksFactory"/>
	/// first.  If Rhino Mocks isn't available, it will attempt to use
	/// <see cref="MoqFactory"/>.
	/// </remarks>
	internal class FirstAvailableMockFactory : IMockFactory
	{
		private readonly IMockFactory _realFactory;

		/// <summary>
		/// Initializes the factory.
		/// </summary>
		public FirstAvailableMockFactory() : this(typeof(RhinoMocksFactory), typeof(MoqFactory))
		{}

		/// <summary>
		/// Creates a factory that will use the first creatable factory type.
		/// </summary>
		/// <param name="factoryTypes"></param>
		internal FirstAvailableMockFactory(params Type[] factoryTypes)
		{
			foreach (Type factoryType in factoryTypes)
			{
				try
				{
					_realFactory = (IMockFactory)Activator.CreateInstance(factoryType);
					break;
				}
				catch (TargetInvocationException ex)
				{
					//Factories should throw an InvalidOperationException if they can't be
					//instantiated due to a missing library.  If that happens, we want to
					//attempt loading the next available factory. 
					if (ex.InnerException is InvalidOperationException)
					{
						continue;
					}

					throw;
				}
			}

			if (_realFactory == null)
			{
				throw new InvalidOperationException("Unable to create a factory.  Be sure a mocking framework is available.");
			}
		}

		/// <summary>
		/// Creates a dynamic mock.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IMockProxy<T> DynamicMock<T>() where T : class
		{
			return _realFactory.DynamicMock<T>();
		}
	}
}