using System;
using System.Web.Mvc;
using MvcContrib.TestHelper.MockFactories;
using NUnit.Framework;

namespace MvcContrib.UnitTests.TestHelper.MockFactories
{
	[TestFixture]
	public class FirstAvailableMockFactoryTests
	{
		[Test]
		public void Should_Default_To_RhinoMocks()
		{
			var factory = new FirstAvailableMockFactory();

			factory.DynamicMock<IController>().GetType().ShouldEqual(typeof(RhinoMocksProxy<IController>));
		}

		private class BadFactory : IMockFactory
		{
			public BadFactory()
			{
				throw new InvalidOperationException();
			}

			public IMockProxy<T> DynamicMock<T>() where T : class
			{
				return null;
			}
		}

		[Test]
		public void Should_Fallback_To_Next_Available_Framework_If_Error_Occurs()
		{
			var factory = new FirstAvailableMockFactory(typeof(BadFactory), typeof(MoqFactory));

			factory.DynamicMock<IController>().GetType().ShouldEqual(typeof(MoqProxy<IController>));
		}
	}
}