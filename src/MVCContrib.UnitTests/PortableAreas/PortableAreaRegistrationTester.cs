using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.PortableAreas;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.PortableAreas
{
	public class when_a_portable_area_has_been_registered
	{
		[TestFixtureSetUp]
		protected void context()
		{
			var registration = new DefaultPortableAreaRegistration();
			var areaContext = new AreaRegistrationContext(registration.AreaName, RouteTable.Routes);
			registration.RegisterArea(areaContext, MockRepository.GenerateStub<IApplicationBus>());
		}

		[Test]
		public void serve_image_from_the_embedded_resource()
		{
			"~/DefaultArea/Images/foo.png".ShouldMapTo<EmbeddedResourceController>(c => c.Index("foo.png", "Content.Images"));
		}

		[Test]
		public void serve_css_from_the_embedded_resource()
		{
			"~/DefaultArea/Styles/bar.css".ShouldMapTo<EmbeddedResourceController>(c => c.Index("bar.css", "Content.Styles"));
		}

		[Test]
		public void serve_javascript_from_the_embedded_resource()
		{
			"~/DefaultArea/Scripts/jquery.js".ShouldMapTo<EmbeddedResourceController>(
				c => c.Index("jquery.js", "Content.Scripts"));
		}

		[TestFixtureTearDown]
		protected void cleanup_routes()
		{
			RouteTable.Routes.Clear();
		}
	}

	public class when_a_portable_area_with_route_prefix_has_been_registered
	{
		[TestFixtureSetUp]
		protected void context()
		{
			var registration = new RoutePrefixedPortableAreaRegistration();
			var areaContext = new AreaRegistrationContext(registration.AreaName, RouteTable.Routes);
			registration.RegisterArea(areaContext, MockRepository.GenerateStub<IApplicationBus>());
		}

		[Test]
		public void serve_image_from_the_embedded_resource()
		{
			"~/ZaaArea.mvc/Images/foo.png".ShouldMapTo<EmbeddedResourceController>(c => c.Index("foo.png", "Content.Images"));
		}

		[Test]
		public void serve_css_from_the_embedded_resource()
		{
			"~/ZaaArea.mvc/Styles/bar.css".ShouldMapTo<EmbeddedResourceController>(c => c.Index("bar.css", "Content.Styles"));
		}

		[Test]
		public void serve_javascript_from_the_embedded_resource()
		{
			"~/ZaaArea.mvc/Scripts/jquery.js".ShouldMapTo<EmbeddedResourceController>(
				c => c.Index("jquery.js", "Content.Scripts"));
		}

		[TestFixtureTearDown]
		protected void cleanup_routes()
		{
			RouteTable.Routes.Clear();
		}
	}

	public class DefaultPortableAreaRegistration : PortableAreaRegistration 
	{
		public override string AreaName
		{
			get { return "DefaultArea"; }
		}
	}

	public class RoutePrefixedPortableAreaRegistration : PortableAreaRegistration
	{
		public override string AreaName
		{
			get { return "BarArea"; }
		}

		public override string AreaRoutePrefix
		{
			get
			{
				return "ZaaArea.mvc";
			}
		}
	}
}