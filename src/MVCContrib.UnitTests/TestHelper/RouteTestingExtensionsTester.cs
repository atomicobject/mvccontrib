using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper;
using NUnit.Framework;
using AssertionException = MvcContrib.TestHelper.AssertionException;

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture]
	public class RouteTestingExtensionsTester
	{
		public class FunkyController : Controller
		{
			public ActionResult Index()
			{
				return null;
			}

			public ActionResult Bar(string id)
			{
				return null;
			}

			public ActionResult New()
			{
				return null;
			}

			public ActionResult List(Bar bar)
			{
				return null;
			}

			public ActionResult Foo(int id)
			{
				return null;
			}

			[AcceptVerbs(HttpVerbs.Post)]
			public ActionResult Zordo(int id)
			{
				return null;
			}

			public ActionResult Guid(Guid id)
			{
				return null;
			}

			public ActionResult Nullable(int? id)
			{
				return null;
			}

			public ActionResult DateTime(DateTime id)
			{
				return null;
			}

			public ActionResult NullableDateTime(DateTime? id)
			{
				return null;
			}

			[ActionName("ActionName")]
			public ActionResult MethodNameDoesntMatch()
			{
				return null;
			}

			public ActionResult ParameterNameDoesntMatch(string someParameter)
			{
				return null;
			}
		}

		public class OptionalExampleController : Controller
		{
			public ActionResult NullableInt(int? id)
			{
				return null;
			}

			public ActionResult String(string id)
			{
				return null;
			}
		}

		public class Bar {}

		public class AwesomeController : Controller {}

		public class HasControllerInMiddleOfNameController : Controller
		{
			public ActionResult Index() { return null; }
		}

		[SetUp]
		public void Setup()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.IgnoreRoute("{resource}.gif/{*pathInfo}");
			RouteTable.Routes.MapRoute(
				"optional-nullable",
				"optional/nullableint/{id}",
				new {controller = "OptionalExample", Action = "NullableInt", id = UrlParameter.Optional}
				);
			RouteTable.Routes.MapRoute(
				"optional-string",
				"optional/string/{id}",
				new {controller = "OptionalExample", Action = "String", id = UrlParameter.Optional}
				);

			RouteTable.Routes.MapRoute(
				"default",
				"{controller}/{action}/{id}",
				new {controller = "Funky", Action = "Index", id = ""});


		}

		[TearDown]
		public void TearDown()
		{
			RouteTable.Routes.Clear();
		}

		[Test]
		public void should_be_able_to_pull_routedata_from_a_string()
		{
			var routeData = "~/charlie/brown".Route();
			Assert.That(routeData, Is.Not.Null);

			Assert.That(routeData.Values.ContainsValue("charlie"));
			Assert.That(routeData.Values.ContainsValue("brown"));
		}

		[Test]
		public void should_be_able_to_match_controller_from_route_data()
		{
			"~/".Route().ShouldMapTo<FunkyController>();
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void should_be_able_to_detect_when_a_controller_doesnt_match()
		{
			"~/".Route().ShouldMapTo<AwesomeController>();
		}

		[Test]
		public void should_be_able_to_match_action_with_lambda()
		{
			"~/".Route().ShouldMapTo<FunkyController>(x => x.Index());
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void should_be_able_to_detect_an_incorrect_action()
		{
			"~/".Route().ShouldMapTo<FunkyController>(x => x.New());
		}

		[Test]
		public void should_be_able_to_match_action_parameters()
		{
			"~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("widget"));
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void should_be_able_to_detect_invalid_action_parameters()
		{
			"~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("something_else"));
		}

		[Test, ExpectedException(typeof(AssertionException), ExpectedMessage = "Value for parameter 'id' did not match: expected 'something_else' but was 'widget'.")]
		public void should_provide_detailed_exception_message_when_detecting_invalid_action_parameters()
		{
			"~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("something_else"));
		}

		[Test]
		public void should_be_able_to_test_routes_directly_from_a_string()
		{
			"~/funky/bar/widget".ShouldMapTo<FunkyController>(x => x.Bar("widget"));
		}

		[Test]
		public void should_be_able_to_test_routes_with_member_expressions_being_used()
		{
			var widget = "widget";

			"~/funky/bar/widget".ShouldMapTo<FunkyController>(x => x.Bar(widget));
		}

		[Test]
		public void should_be_able_to_test_routes_with_member_expressions_being_used_but_ignore_null_complex_parameteres()
		{
			"~/funky/List".ShouldMapTo<FunkyController>(x => x.List(null));
		}

		[Test]
		public void should_be_able_to_ignore_requests()
		{
			"~/someimage.gif".ShouldBeIgnored();
		}

		[Test]
		public void should_be_able_to_ignore_requests_with_path_info()
		{
			"~/someimage.gif/with_stuff".ShouldBeIgnored();
		}

		[Test]
		public void should_be_able_to_match_non_string_action_parameters()
		{
			"~/funky/foo/1234".Route().ShouldMapTo<FunkyController>(x => x.Foo(1234));
		}

		[Test]
		public void assertion_exception_should_hide_the_test_helper_frames_in_the_call_stack()
		{
			IEnumerable<string> callstack = new string[0];
			try
			{
				"~/badroute that is not configures/foo/1234".Route().ShouldMapTo<FunkyController>(x => x.Foo(1234));
			}
			catch(Exception ex)
			{
				callstack = ex.StackTrace.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
			}
			callstack.Count().ShouldEqual(1);
		}

		[Test]
		public void should_be_able_to_generate_url_from_named_route()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapRoute(
				"namedRoute",
				"{controller}/{action}/{id}",
				new {controller = "Funky", Action = "Index", id = ""});

			OutBoundUrl.OfRouteNamed("namedRoute").ShouldMapToUrl("/");
		}

		[Test]
		public void should_be_able_to_generate_url_from_controller_action_where_action_is_default()
		{
			OutBoundUrl.Of<FunkyController>(x => x.Index()).ShouldMapToUrl("/");
		}

		[Test]
		public void should_be_able_to_generate_url_from_controller_action()
		{
			OutBoundUrl.Of<FunkyController>(x => x.New()).ShouldMapToUrl("/Funky/New");
		}

		[Test]
		public void should_be_able_to_generate_url_from_controller_action_with_parameter()
		{
			OutBoundUrl.Of<FunkyController>(x => x.Foo(1)).ShouldMapToUrl("/Funky/Foo/1");
		}

		[Test]
		public void should_be_able_to_match_action_with_lambda_and_httpmethod()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapRoute(
				"zordoRoute",
				"{controller}/{action}/{id}",
				new {controller = "Funky", Action = "Zordo", id = "0"},
				new {httpMethod = new HttpMethodConstraint("POST")});
			"~/Funky/Zordo/0".WithMethod(HttpVerbs.Post).ShouldMapTo<FunkyController>(x => x.Zordo(0));
		}

		[Test]
		public void should_not_be_able_to_get_routedata_with_wrong_httpmethod()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapRoute(
				"zordoRoute",
				"{controller}/{action}/{id}",
				new {controller = "Funky", Action = "Zordo", id = "0"},
				new {httpMethod = new HttpMethodConstraint("POST")});
			var routeData = "~/Funky/Zordo/0".WithMethod(HttpVerbs.Get);
			Assert.IsNull(routeData);
		}

		[Test]
		public void should_match_guid()
		{
			"~/funky/guid/80e70232-e660-40ae-af6b-2b2b8e87ee48".Route().ShouldMapTo<FunkyController>(c => c.Guid(new Guid("80e70232-e660-40ae-af6b-2b2b8e87ee48")));
		}

		[Test]
		public void should_match_nullable_int()
		{
			"~/funky/nullable/24".Route().ShouldMapTo<FunkyController>(c => c.Nullable(24));
		}

		[Test]
		public void should_match_nullable_int_when_null()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.IgnoreRoute("{resource}.gif/{*pathInfo}");
			RouteTable.Routes.MapRoute(
				"default",
				"{controller}/{action}/{id}",
				new {controller = "Funky", Action = "Index", id = (int?)null});

			"~/funky/nullable".Route().ShouldMapTo<FunkyController>(c => c.Nullable(null));
		}

		[Test]
		public void should_be_able_to_generate_url_with_nullable_int_action_parameter()
		{
			OutBoundUrl.Of<FunkyController>(c => c.Nullable(24)).ShouldMapToUrl("/funky/nullable/24");
		}

		[Test]
		public void should_be_able_to_generate_url_with_optional_nullable_int_action_parameter()
		{
			OutBoundUrl.Of<OptionalExampleController>(c => c.NullableInt(24))
				.ShouldMapToUrl("/optional/nullableint/24");
		}

		[Test]
		public void should_be_able_to_generate_url_with_optional_nullable_int_action_parameter_with_null()
		{
			OutBoundUrl.Of<OptionalExampleController>(c => c.NullableInt(null))
				.ShouldMapToUrl("/optional/nullableint");
		}

		[Test]
		public void should_be_able_to_generate_url_with_optional_string_action_parameter()
		{
			OutBoundUrl.Of<OptionalExampleController>(c => c.String("foo"))
				.ShouldMapToUrl("/optional/string/foo");
		}

		[Test]
		public void should_be_able_to_generate_url_with_optional_string_action_parameter_with_empty_string()
		{
			OutBoundUrl.Of<OptionalExampleController>(c => c.String(""))
				.ShouldMapToUrl("/optional/string");
		}

		[Test]
		public void should_be_able_to_generate_url_with_optional_string_action_parameter_with_null()
		{
			OutBoundUrl.Of<OptionalExampleController>(c => c.String(null))
				.ShouldMapToUrl("/optional/string");
		}

		[Test]
		public void should_match_datetime()
		{
			"~/funky/DateTime/2009-1-1".Route().ShouldMapTo<FunkyController>(x => x.DateTime(new DateTime(2009, 1, 1)));
		}

		[Test]
		public void should_match_nullabledatetime()
		{
			"~/funky/NullableDateTime/2009-1-1".Route().ShouldMapTo<FunkyController>(x => x.NullableDateTime(new DateTime(2009, 1, 1)));
		}

		[Test]
		public void should_match_method_with_different_name_than_action()
		{
			"~/funky/ActionName".Route().ShouldMapTo<FunkyController>(x => x.MethodNameDoesntMatch());
		}

		[Test]
		public void should_be_able_to_match_optional_parameter_against_a_lambda_with_a_nullable_missing_expected_value()
		{
			"~/optional/nullableint".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.NullableInt(null));
		}

		[Test]
		public void should_be_able_to_match_optional_parameter_with_a_slash_against_a_lambda_with_a_nullable_missing_expected_value()
		{
			"~/optional/nullableint/".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.NullableInt(null));
		}

		[Test]
		public void should_be_able_to_match_optional_parameter_against_a_lambda_with_a_nullable_correct_expected_value()
		{
			"~/optional/nullableint/3".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.NullableInt(3));
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void should_throw_with_match_optional_parameter_against_a_lambda_with_a_nullable_incorrect_expected_value()
		{
			"~/optional/nullableint/5".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.NullableInt(3));
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void should_throw_with_optional_string_parameter_against_a_lambda_with_an_actual_expected_value()
		{
			"~/optional/string".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.String("charlie"));
		}

		[Test]
		public void should_be_able_to_match_optional_string_parameter_against_a_lambda_with_a_nullable_missing_expected_value()
		{
			"~/optional/string".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.String(null));
		}

		[Test]
		public void should_be_able_to_match_optional_string_parameter_with_a_slash_against_a_lambda_with_a_nullable_missing_expected_value()
		{
			"~/optional/string/".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.String(null));
		}

		[Test]
		public void should_be_able_to_match_optional_string_parameter_against_a_lambda_with_a_nullable_correct_expected_value()
		{
			"~/optional/string/foo".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.String("foo"));
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void should_throw_with_an_optional_string_parameter_against_a_lambda_with_a_nullable_incorrect_expected_value()
		{
			"~/optional/string/bar".Route()
				.ShouldMapTo<OptionalExampleController>(x => x.String("foo"));
		}

		[Test, ExpectedException(typeof(AssertionException), ExpectedMessage = "Value for parameter 'someParameter' did not match: expected 'foo' but was ''; no value found in the route context action parameter named 'someParameter' - does your matching route contain a token called 'someParameter'?")]
		public void should_provide_detailed_exception_message_when_detecting_a_parameter_name_that_doesnt_match()
		{
			"~/funky/parameterNameDoesntMatch/foo".Route().ShouldMapTo<FunkyController>(x => x.ParameterNameDoesntMatch("foo"));
		}

		[Test]
		public void ShouldMapToPage_detects_route_is_for_webforms()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapPageRoute("webform-page", "web/forms/route", "~/MyPage.aspx");

			"~/web/forms/route".ShouldMapToPage("~/MyPage.aspx");
		}

		[Test]
		public void ShouldMapToPage_Throws_when_WebForm_route_maps_to_wrong_page()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapPageRoute("webform-page", "web/forms/route", "~/MyPage.aspx");

			Assert.Throws<MvcContrib.TestHelper.AssertionException>(() => "~/web/forms/route".ShouldMapToPage("~/Foo.aspx"));
			
		}

		[Test]
		public void ShouldMapToPage_throws_when_route_does_not_use_PageRouteHandler()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapRoute("broken-route", "web/forms/route", new { controller = "Broken", action = "Index" });

			
			Assert.Throws<MvcContrib.TestHelper.AssertionException>(() => "~/web/forms/route".ShouldMapToPage("~/MyPage.aspx"));

		}

		[Test]
		public void ShouldMapTo_DoesNotStripControllerFromMiddleOfName()
		{
			RouteTable.Routes.Clear();
			RouteTable.Routes.MapRoute("test", "foo", new { controller = "HasControllerInMiddleOfName", action = "index" });

			"~/foo".ShouldMapTo<HasControllerInMiddleOfNameController>(x => x.Index());
		}
	}
}