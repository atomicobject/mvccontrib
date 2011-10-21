using System.Web.Mvc;
using MvcContrib.TestHelper.MockFactories;
using NUnit.Framework;

namespace MvcContrib.UnitTests.TestHelper.MockFactories
{
	[TestFixture]
	public class RhinoMocksFactoryTests
	{
		private RhinoMocksFactory _factory;

		[SetUp]
		public void Prepare()
		{
			_factory = new RhinoMocksFactory();
		}

		[Test]
		public void Creates_Dynamic_Mock()
		{
			var mock = _factory.DynamicMock<IController>();

			mock.ShouldNotBeNull();
		}

	}
}