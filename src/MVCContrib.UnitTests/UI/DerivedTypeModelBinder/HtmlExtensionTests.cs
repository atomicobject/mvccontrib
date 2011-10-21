using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Binders;
using MvcContrib.UI.DerivedTypeModelBinder;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.DerivedTypeModelBinder
{
	[TestFixture]
	public class HtmlExtensionTests
	{
		private static HtmlHelper<TModel> CreateHtmlHelper<TModel>() where TModel:new()
		{
			var viewDataDictionary = new ViewDataDictionary(new TModel());
			var viewContext = MockRepository.GenerateStub<ViewContext>();
			viewContext.ViewData = viewDataDictionary;
			var viewDataContainer = MockRepository.GenerateStub<IViewDataContainer>();
			viewDataContainer.ViewData = viewDataDictionary;
			var htmlHelper = new HtmlHelper<TModel>(viewContext, viewDataContainer);
			return htmlHelper;
		}

		[Test]
		public void TypeStamp_throws_error_when_requested_class_is_not_registered()
		{
			DerivedTypeModelBinderCache.Reset();

			var html = CreateHtmlHelper<HtmlExtensionTests>();

			Assert.Throws<KeyNotFoundException>(() => html.TypeStamp().ToHtmlString());
		}

		[Test]
		public void TypeStamp_returns_proper_encoded_identifier()
		{
			DerivedTypeModelBinderCache.Reset();

			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(ITestClass), new[] {typeof(TestClass)});

			var html = CreateHtmlHelper<TestClass>();

			var output = html.TypeStamp();

			Assert.That(output.ToHtmlString(), Is.StringContaining("name=\"_xTypeStampx_\""));
			Assert.That(output.ToHtmlString(), Is.StringContaining("value=\""));
			Assert.That(output.ToHtmlString(), Is.StringContaining("type=\"hidden\""));
		}

		[Test]
		public void TypeStamp_uses_type_stamp_name()
		{
			DerivedTypeModelBinderCache.Reset();

			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof(ITestClass), new[] { typeof(TestClass) });

			var originalField = DerivedTypeModelBinderCache.TypeStampFieldName;
			DerivedTypeModelBinderCache.TypeStampFieldName = "test";

			var html = CreateHtmlHelper<TestClass>();

			var output = html.TypeStamp();

			DerivedTypeModelBinderCache.TypeStampFieldName = originalField;

			Assert.That(output.ToHtmlString(), Is.StringContaining("name=\"test\""));

			
		}
		

	}
}
