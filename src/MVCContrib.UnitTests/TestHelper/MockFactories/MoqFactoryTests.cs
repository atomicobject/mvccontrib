using System.Web.Mvc;
using MvcContrib.TestHelper.MockFactories;
using NUnit.Framework;

namespace MvcContrib.UnitTests.TestHelper.MockFactories
{
	[TestFixture]
	public class MoqFactoryTests
	{
		private MoqFactory _factory;

		[SetUp]
		public void Prepare()
		{
			_factory = new MoqFactory();
		}

		[Test]
		public void Creates_Dynamic_Mock()
		{
			var mock = _factory.DynamicMock<IController>();

			mock.ShouldNotBeNull();
		}
	}
}