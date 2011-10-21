using System.ComponentModel;
using MvcContrib.UI.InputBuilder.Conventions;
using MvcContrib.UI.InputBuilder.InputSpecification;
using MvcContrib.UI.InputBuilder.Views;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class IInputSpecificationExtensionsTester
	{
		[Test]
		public void methods_should_modify_the_underlying_model()
		{
			//arrange
			var inputSpecification = new InputPropertySpecification();
			inputSpecification.Model = new PropertyViewModel();

			//act
			inputSpecification
				.MaxLength(5)
				.Example("new example")
				.Required()
				.Partial("partial")
				.Label("label");


			//assert
			Assert.AreEqual(5, inputSpecification.Model.AdditionalValues["maxlength"]);
			Assert.AreEqual("partial", inputSpecification.Model.PartialName);
			Assert.AreEqual("new example", inputSpecification.Model.Example);
			Assert.AreEqual("label", inputSpecification.Model.Label);
			Assert.IsTrue(inputSpecification.Model.PropertyIsRequired);
		}

		[Test]
		public void Gets_label_from_DisplayNameAttribute()
		{
			var conventions = new DefaultPropertyConvention();
			var model = conventions.Create(typeof(TestModel).GetProperty("Name"), new TestModel(), "Name", typeof(TestModel));
			model.Label.ShouldEqual("Foo");
		}

		private class TestModel
		{
			[DisplayName("Foo")]
			public string Name { get; set; }
		}
	}

	public static class SetUserExtensions
	{
		public static IInputSpecification<PropertyViewModel> MaxLength(
			this IInputSpecification<PropertyViewModel> inputSpecification, int length)
		{
			inputSpecification.Model.AdditionalValues.Add("maxlength", length);
			return inputSpecification;
		}
	}
}