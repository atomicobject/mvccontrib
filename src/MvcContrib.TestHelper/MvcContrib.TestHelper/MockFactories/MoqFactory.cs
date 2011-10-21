using System;
using System.Reflection;

namespace MvcContrib.TestHelper.MockFactories
{
    /// <summary>
    /// Creates mock objects using Moq.
    /// </summary>
    internal class MoqFactory : IMockFactory
    {
		private static readonly Type _mockOpenType;
    	private static readonly Exception _loadException;

		/// <summary>
		/// Grabs references to static types.
		/// </summary>
    	static MoqFactory()
		{
			try
			{
				Assembly moq = Assembly.Load("Moq");
				_mockOpenType = moq.GetType("Moq.Mock`1");

				if (_mockOpenType == null)
				{
					throw new InvalidOperationException("Unable to find Type Moq.Mock<T> in assembly " + moq.Location);
				}				
			}
			catch(Exception ex)
			{
				_loadException = ex;
			}
		}

		/// <summary>
		/// Creates a new factory.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if Moq isn't available.</exception>
		public MoqFactory()
		{
			if (_mockOpenType == null)
			{
				throw new InvalidOperationException("Unable to create a proxy for Moq.", _loadException);
			}
		}

		/// <summary>
		/// Creates a new dynamic mock.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IMockProxy<T> DynamicMock<T>() where T : class
		{
			return new MoqProxy<T>(_mockOpenType.MakeGenericType(typeof(T)));
		}
	}
}