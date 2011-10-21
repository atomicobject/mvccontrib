using System;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Services;
using MvcContrib.TestHelper;
using MvcContrib.TestHelper.MockFactories;
using NUnit.Framework;
using Rhino.Mocks;

//Note: these tests confirm that the TestControllerBuilder works properly
//with both Rhino Mocks and Moq. 
//For examples on how to use the TestControllerBuilder and other TestHelper classes,
//look in the \src\Samples\MvcContrib.TestHelper solution

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture(typeof(MoqFactory))]
	[TestFixture(typeof(RhinoMocksFactory))]
	public class ControllerBuilderTests<TMockFactory> where TMockFactory : IMockFactory, new()
	{
		private TestControllerBuilder _builder;

		[SetUp]
		public void Setup()
		{
			_builder = new TestControllerBuilder(new TMockFactory());
		}

		[Test]
		public void CanSpecifyFiles()
		{
			var mocks = new MockRepository();
			var file = mocks.DynamicMock<HttpPostedFileBase>();

			_builder.Files["Variable"] = file;
			var controller = new TestHelperController();
			_builder.InitializeController(controller);
			Assert.AreSame(file, controller.Request.Files["Variable"]);
		}

		[Test]
		public void CanSpecifyFormVariables()
		{
			_builder.Form["Variable"] = "Value";
			var controller = new TestHelperController();
			_builder.InitializeController(controller);
			Assert.AreEqual("Value", controller.HttpContext.Request.Form["Variable"]);
		}

		[Test]
		public void CanSpecifyRouteData()
		{
			var rd = new RouteData();
			rd.Values["Variable"] = "Value";
			_builder.RouteData = rd;

			var controller = new TestHelperController();
			_builder.InitializeController(controller);
			Assert.AreEqual("Value", controller.RouteData.Values["Variable"]);
		}

		[Test]
		public void CanSpecifyQueryString()
		{
			_builder.QueryString["Variable"] = "Value";
			var testController = new TestHelperController();
			_builder.InitializeController(testController);
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
		}

		[Test]
		public void CanSpecifyAppRelativeCurrentExecutionFilePath()
		{
			_builder.AppRelativeCurrentExecutionFilePath = "someUrl";
			var testController = new TestHelperController();
			_builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.AppRelativeCurrentExecutionFilePath);
		}

		[Test]
		public void CanSpecifyApplicationPath()
		{
			_builder.ApplicationPath = "someUrl";
			var testController = new TestHelperController();
			_builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.ApplicationPath);
		}

		[Test]
		public void CanSpecifyPathInfol()
		{
			_builder.PathInfo = "someUrl";
			var testController = new TestHelperController();
			_builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.PathInfo);
		}

		[Test]
		public void CanSpecifyRawUrl()
		{
			_builder.RawUrl = "someUrl";
			var testController = new TestHelperController();
			_builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.RawUrl);
		}

		[Test]
		public void CanSpecifyRequestAcceptTypes()
		{
			_builder.AcceptTypes = new[] {"some/type-extension"};
			var controller = new TestHelperController();
			_builder.InitializeController(controller);
			Assert.That(controller.HttpContext.Request.AcceptTypes, Is.Not.Null);
			Assert.That(controller.HttpContext.Request.AcceptTypes.Length, Is.EqualTo(1));
			Assert.That(controller.HttpContext.Request.AcceptTypes[0], Is.EqualTo("some/type-extension"));
		}

		[Test]
		public void When_response_status_is_set_it_should_persist()
		{
			var testController = new TestHelperController();
			_builder.InitializeController(testController);
			testController.HttpContext.Response.Status = "404 Not Found";
			Assert.AreEqual("404 Not Found", testController.HttpContext.Response.Status);
		}

		[Test]
		public void CanCreateControllerWithNoArgs()
		{
			_builder.QueryString["Variable"] = "Value";
			var testController = _builder.CreateController<TestHelperController>();
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
		}

		[Test]
		public void When_params_is_invoked_it_should_return_a_combination_of_form_and_querystring()
		{
			_builder.QueryString["foo"] = "bar";
			_builder.Form["baz"] = "blah";
			var testController = new TestHelperController();
			_builder.InitializeController(testController);
			Assert.That(testController.Request.Params["foo"], Is.EqualTo("bar"));
			Assert.That(testController.Request.Params["baz"], Is.EqualTo("blah"));
		}

		[Test]
		public void CanCreateControllerWithArgs()
		{
			_builder.QueryString["Variable"] = "Value";
			var testController = _builder.CreateController<TestHelperWithArgsController>(new TestService());
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
			Assert.AreEqual("Moo", testController.ReturnMooFromService());
		}

		[Test]
		public void CanCreateControllerWithIoCArgs()
		{
			var mocks = new MockRepository();
			using(mocks.Record())
			{
				var resolver = mocks.DynamicMock<IDependencyResolver>();
				Expect.Call(resolver.GetService(typeof(TestHelperWithArgsController))).Return(
					new TestHelperWithArgsController(new TestService()));
				DependencyResolver.SetResolver(resolver);
			}
			using(mocks.Playback())
			{
				_builder.QueryString["Variable"] = "Value";
				var testController = _builder.CreateIoCController<TestHelperWithArgsController>();
				Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
				Assert.AreEqual("Moo", testController.ReturnMooFromService());
			}
		}

		[Test]
		public void UserShouldBeMocked()
		{
			var mocks = new MockRepository();
			var user = mocks.DynamicMock<IPrincipal>();

			var controller = _builder.CreateController<TestHelperController>();
			controller.ControllerContext.HttpContext.User = user;

			Assert.AreSame(user, controller.User);
		}

		[Test]
		public void CacheIsAvailable()
		{
			Assert.IsNotNull(_builder.HttpContext.Cache);

			var controller = new TestHelperController();
			_builder.InitializeController(controller);

			Assert.IsNotNull(controller.HttpContext.Cache);

			string testKey = "TestKey";
			string testValue = "TestValue";

			controller.HttpContext.Cache.Add(testKey,
			                                 testValue,
			                                 null,
			                                 DateTime.Now.AddSeconds(1),
			                                 Cache.NoSlidingExpiration,
			                                 CacheItemPriority.Normal,
			                                 null);

			Assert.AreEqual(testValue,
			                controller.HttpContext.Cache[testKey]);
		}

		[Test]
		public void Initializes_UrlHelper()
		{
			var controller = _builder.CreateController<TestHelperController>();
			controller.Url.ShouldNotBeNull();
		}

		[Test]
		public void Initializes_ServerVariables()
		{
			var controller = _builder.CreateController<TestHelperController>();
			controller.Request.ServerVariables.ShouldNotBeNull();
		}
	}
}
