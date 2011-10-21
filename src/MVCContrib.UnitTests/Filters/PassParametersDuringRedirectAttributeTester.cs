using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.ActionResults;
using MvcContrib.Filters;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class PassParametersDuringRedirectAttributeTester
	{
		private PassParametersDuringRedirectAttribute _filter;
		private SomeReferenceType _someReferenceType;

		[SetUp]
		public void Setup()
		{
			_filter = new PassParametersDuringRedirectAttribute();
			_someReferenceType = new SomeReferenceType {One = 1, Two = "two"};
		}

		[Test]
		public void OnActionExecuting_should_load_the_parameter_values_out_of_TempData_when_they_match_both_name_and_type_of_a_parameter_of_the_action_being_executed()
		{
			var context = new ActionExecutingContext
			{
				Controller = new SampleController(),
				ActionParameters = new Dictionary<string, object>(),
				ActionDescriptor = GetActionDescriptorStubForIndexAction()
			};

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"] = _someReferenceType;

			_filter.OnActionExecuting(context);

			context.ActionParameters["viewModel"].ShouldEqual(_someReferenceType);
		}

		[Test]
		public void OnActionExecuting_should_load_the_parameter_values_out_of_TempData_when_they_match_the_name_and_are_assignable_to_the_type_of_a_parameter_of_the_action_being_executed()
		{
			var objectAssignableToSomeObject = new ReferenceTypeAssignableToSomeReferenceType();

			var context = new ActionExecutingContext
			{
				Controller = new SampleController(),
				ActionParameters = new Dictionary<string, object>(),
				ActionDescriptor = GetActionDescriptorStubForIndexAction()
			};

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"] = objectAssignableToSomeObject;

			_filter.OnActionExecuting(context);

			context.ActionParameters["viewModel"].ShouldEqual(objectAssignableToSomeObject);
		}

		[Test]
		public void OnActionExecuting_should_not_load_the_parameter_values_out_of_TempData_which_do_not_have_matching_name_and_assignable_type_of_a_parameter_of_the_action_being_executed()
		{
			var context = new ActionExecutingContext
			{
				Controller = new SampleController(),
				ActionParameters = new Dictionary<string, object>(),
				ActionDescriptor = GetActionDescriptorStubForIndexAction()
			};

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "theNameOfThisParameterDoesNotMatch"] = _someReferenceType;
			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"] = "the type of this parameter does not match";

			_filter.OnActionExecuting(context);

			context.ActionParameters.ContainsKey("theNameOfThisParameterDoesNotMatch").ShouldBeFalse();
			context.ActionParameters.ContainsKey("viewModel").ShouldBeFalse();
		}

		[Test]
		public void OnActionExecuting_should_not_load_null_parameter_values()
		{
			var context = new ActionExecutingContext
			{
				Controller = new SampleController(),
				ActionParameters = new Dictionary<string, object>(),
				ActionDescriptor = GetActionDescriptorStubForIndexAction()
			};

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"] = null;

			_filter.OnActionExecuting(context);

			context.ActionParameters.ContainsKey("viewModel").ShouldBeFalse();
		}


		[Test]
		public void Matching_stored_parameters_values_should_be_kept_in_TempData()
		{
			var context = new ActionExecutingContext
			{
				Controller = new SampleController(),
				ActionParameters = new Dictionary<string, object>(),
				ActionDescriptor = GetActionDescriptorStubForIndexAction()
			};

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"] = _someReferenceType;

			_filter.OnActionExecuting(context);
			context.Controller.TempData.Save(context, MockRepository.GenerateStub<ITempDataProvider>());

			context.Controller.TempData.ContainsKey(PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel").ShouldBeTrue();
		}

		[Test]
		public void Non_matching_stored_parameter_values_should_be_not_be_kept_in_TempData()
		{
			var actionDescriptorWithNoParameters = MockRepository.GenerateStub<ActionDescriptor>();
			actionDescriptorWithNoParameters.Stub(ad => ad.GetParameters()).Return(new ParameterDescriptor[] {});

			var context = new ActionExecutingContext
			{
				Controller = new SampleController(),
				ActionParameters = new Dictionary<string, object>(),
				ActionDescriptor = actionDescriptorWithNoParameters
			};

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"] = _someReferenceType;

			_filter.OnActionExecuting(context);
			context.Controller.TempData.Save(context, MockRepository.GenerateStub<ITempDataProvider>());

			context.Controller.TempData.ContainsKey(PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel").ShouldBeFalse();
		}

		[Test]
		public void OnActionExecuted_should_store_reference_type_parameters_in_tempdata_when_result_is_generic_RedirectToRouteResult()
		{
			var context = new ActionExecutedContext
			{
				Result = new RedirectToRouteResult<SampleController>(x => x.Action1(_someReferenceType, 5)),
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);
			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"].ShouldEqual(_someReferenceType);
		}

		[Test]
		public void OnActionExecuted_should_store_value_type_parameters_where_the_value_type_cannot_be_converted_from_string()
		{
			var someValueType = new SomeValueTypeWhichCannotBeConvertedFromString(1, "two");

			var context = new ActionExecutedContext
			{
				Result = new RedirectToRouteResult<SampleController>(x => x.Action3(someValueType)),
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"].ShouldEqual(someValueType);
		}

		[Test]
		public void OnActionExecuted_should_not_store_value_type_parameters_where_the_value_type_can_be_converted_from_string()
		{
			var context = new ActionExecutedContext
			{
				Result = new RedirectToRouteResult<SampleController>(x => x.Action1(_someReferenceType, 5)),
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "id"].ShouldBeNull();
		}

		[Test]
		public void OnActionExecuted_should_not_store_string_parameters()
		{
			var context = new ActionExecutedContext
			{
				Result = new RedirectToRouteResult<SampleController>(x => x.Action2("foo")),
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "searchTerm"].ShouldBeNull();
		}

		[Test]
		public void Should_not_store_null_parameters()
		{
			var context = new ActionExecutedContext
			{
				Result = new RedirectToRouteResult<SampleController>(x => x.Action1(null, 5)),
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);

			context.Controller.TempData[PassParametersDuringRedirectAttribute.RedirectParameterPrefix + "viewModel"].ShouldBeNull();
		}

		[Test]
		public void Should_remove_items_from_routevalues_once_stored_in_tempdata()
		{
			var result = new RedirectToRouteResult<SampleController>(x => x.Action1(_someReferenceType, 5));
			var context = new ActionExecutedContext
			{
				Result = result,
				Controller = new SampleController()
			};

			_filter.OnActionExecuted(context);
			result.RouteValues.ContainsKey("viewModel").ShouldBeFalse();
		}

		public class SomeReferenceType
		{
			public int One { get; set; }
			public string Two { get; set; }
		}

		public class ReferenceTypeAssignableToSomeReferenceType : SomeReferenceType {}

		public struct SomeValueTypeWhichCannotBeConvertedFromString
		{
			public SomeValueTypeWhichCannotBeConvertedFromString(int one, string two)
				: this()
			{
				One = one;
				Two = two;
			}

			public int One { get; private set; }
			public string Two { get; private set; }
		}

		public class SampleController : Controller
		{
			public ActionResult Action1(SomeReferenceType viewModel, int id)
			{
				return View(viewModel);
			}

			public ActionResult Action2(string searchTerm)
			{
				return this.RedirectToAction(c => c.Action1(new SomeReferenceType(), 1));
			}

			public ActionResult Action3(SomeValueTypeWhichCannotBeConvertedFromString viewModel)
			{
				return View(viewModel);
			}
		}


		private static ActionDescriptor GetActionDescriptorStubForIndexAction()
		{
			var firstParameterDescriptor = MockRepository.GenerateStub<ParameterDescriptor>();
			firstParameterDescriptor.Stub(pd => pd.ParameterName).Return("viewModel");
			firstParameterDescriptor.Stub(pd => pd.ParameterType).Return(typeof(SomeReferenceType));

			var secondParameterDescriptor = MockRepository.GenerateStub<ParameterDescriptor>();
			secondParameterDescriptor.Stub(pd => pd.ParameterName).Return("id");
			secondParameterDescriptor.Stub(pd => pd.ParameterType).Return(typeof(int));

			var actionDescriptor = MockRepository.GenerateStub<ActionDescriptor>();
			actionDescriptor.Stub(descriptor => descriptor.GetParameters()).Return(new[] {firstParameterDescriptor, secondParameterDescriptor});
			return actionDescriptor;
		}
	}
}