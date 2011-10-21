using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using HtmlAttribute=MvcContrib.FluentHtml.Html.HtmlAttribute;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ValidationMemberBehaviorTests
	{
		ModelStateDictionary stateDictionary;
		private Expression<Func<FakeModel, object>> expression;
		private ValidationBehavior target;

		[SetUp]
		public void SetUp()
		{
			stateDictionary = new ModelStateDictionary();
			expression = null;
			target = new ValidationBehavior(() => stateDictionary);
		}

		[Test]
		public void element_for_member_with_no_error_renders_with_no_class()
		{
			expression = x => x.Price;
			var textbox = new TextBox(expression.GetNameFor(), null, new List<IBehaviorMarker> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldNotHaveAttribute(HtmlAttribute.Class);
		}

		[Test]
		public void element_for_member_with_error_renders_with_default_error_class()
		{
			stateDictionary.AddModelError("Price", "Something bad happened");
			expression = x => x.Price;
			var textbox = new TextBox(expression.GetNameFor(), null, new List<IBehaviorMarker> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute(HtmlAttribute.Class).WithValue("input-validation-error");
		}

		[Test]
		public void element_for_member_with_error_renders_with_specified_error_class_and_specified_other_class()
		{
			stateDictionary.AddModelError("Price", "Something bad happened");
			target = new ValidationBehavior(() => stateDictionary, "my-error-class");
			expression = x => x.Price;
			var textbox = new TextBox(expression.GetNameFor(), null, new List<IBehaviorMarker> { target })
				.Class("another-class");
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute(HtmlAttribute.Class).Value
				.ShouldContain("another-class")
				.ShouldContain("my-error-class");
		}

		[Test]
		public void element_with_error_renders_with_attempted_value()
		{
			stateDictionary.AddModelError("Price", "Something bad happened");
			stateDictionary["Price"].Value = new ValueProviderResult("bad value", "bad value", CultureInfo.CurrentCulture);
			expression = x => x.Price;
			var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(),
				new List<IBehaviorMarker> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("bad value");
		}

		[Test]
		public void element_without_error_renders_with_attempted_value()
		{
			stateDictionary.Add("Price", new ModelState() { Value = new ValueProviderResult("foo", "foo", CultureInfo.CurrentCulture) });

			expression = x => x.Price;
			var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), new List<IBehaviorMarker> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("foo");

		}

		[Test]
		public void does_not_add_css_class_when_retrieving_value_from_modelstate_with_no_error()
		{
			stateDictionary.Add("Price", new ModelState() { Value = new ValueProviderResult("foo", "foo", CultureInfo.CurrentCulture) });
			
			expression = x => x.Price;
			var textbox = new TextBox(expression.GetNameFor(), null, new List<IBehaviorMarker> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");

			element.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("foo");
			element.ShouldNotHaveAttribute(HtmlAttribute.Class);
		}

		[Test]
		public void handles_checkboxes_correctly()
		{
			stateDictionary.AddModelError("Done", "Foo");
			stateDictionary["Done"].Value = new ValueProviderResult(new[] { "true", "false" }, "true", CultureInfo.CurrentCulture);
			expression = x => x.Done;
			var checkbox = new CheckBox(expression.GetNameFor(), expression.GetMemberExpression(), new List<IBehaviorMarker> { target });
			var element = checkbox.ToString().ShouldHaveHtmlNode("Done");
			element.ShouldHaveAttribute("checked").WithValue("checked");
			element.ShouldHaveAttribute("value").WithValue("true");
		}

		[Test]
		public void when_handling_checkbox_does_not_fall_back_to_default_behavior()
		{
			stateDictionary.AddModelError("Done", "Foo");
			stateDictionary["Done"].Value = new ValueProviderResult(new[] { "false", "false" }, "false", CultureInfo.CurrentCulture);
			expression = x => x.Done;
			var checkbox = new CheckBox(expression.GetNameFor(), expression.GetMemberExpression(), new List<IBehaviorMarker> { target });
			var element = checkbox.ToString().ShouldHaveHtmlNode("Done");
			element.ShouldHaveAttribute("value").WithValue("true");
		}

		[Test]
		public void does_not_restore_value_for_password_field()
		{
			stateDictionary.Add("Password", new ModelState() { Value = new ValueProviderResult("foo", "foo", CultureInfo.CurrentCulture) });

			expression = x => x.Password;
			var passwordField = new Password(expression.GetNameFor(), expression.GetMemberExpression(), new List<IBehaviorMarker> { target });
			var element = passwordField.ToString().ShouldHaveHtmlNode("Password");
			element.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("");
		}

		[Test]
		public void restore_checked_from_radio_set()
		{
			stateDictionary.Add("Selection", new ModelState { Value = new ValueProviderResult((int)FakeEnum.Two, "2", CultureInfo.CurrentCulture) });
			expression = x => x.Selection;

			var html = new RadioSet(expression.GetNameFor(), expression.GetMemberExpression(), new List<IBehaviorMarker> { target }).Options<FakeEnum>().ToString();
			var element = html.ShouldHaveHtmlNode("Selection");
			var options = element.ShouldHaveChildNodesCount(8);

			RadioSetTests.VerifyOption("Selection", (int)FakeEnum.Zero, FakeEnum.Zero, options[0], options[1],false);
			RadioSetTests.VerifyOption("Selection", (int)FakeEnum.One, FakeEnum.One, options[2], options[3],false);
			RadioSetTests.VerifyOption("Selection", (int)FakeEnum.Two, FakeEnum.Two, options[4], options[5],true);
			RadioSetTests.VerifyOption("Selection", (int)FakeEnum.Three, FakeEnum.Three, options[6], options[7],false);
		}

		[Test]
		public void Should_apply_model_state_value_to_textbox_value()
		{
			stateDictionary.SetModelValue("foo", new ValueProviderResult("bar", "bar", CultureInfo.InvariantCulture));
			var textbox = new TextBox("foo");
			target.Execute(textbox);
			textbox.ToString()
				.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.Value)
				.Value
				.ShouldEqual("bar");
		}

		[Test]
		public void Should_apply_model_state_value_to_textarea_value()
		{
			stateDictionary.SetModelValue("foo", new ValueProviderResult("bar", "bar", CultureInfo.InvariantCulture));
			var textArea = new TextArea("foo");
			target.Execute(textArea);
			textArea.ToString()
				.ShouldHaveHtmlNode("foo")
				.ShouldHaveInnerTextEqual("bar");
		}

		[Test]
		public void Should_apply_model_state_value_to_select_element_selected_value()
		{
			stateDictionary.SetModelValue("foo", new ValueProviderResult(1, "1", CultureInfo.InvariantCulture));
			var select = new Select("foo").Options(new[] { 1, 2 });
			target.Execute(select);
			var selectedValues = ConvertToGeneric<string>(select.SelectedValues);
			select.SelectedValues.ShouldNotBeNull();
			selectedValues.ShouldCount(1);
			Assert.Contains("1", selectedValues);
		}

		[Test]
		public void Should_apply_model_state_values_to_multiselect_element_selected_values()
		{
			stateDictionary.SetModelValue("foo", new ValueProviderResult(new[] { 1, 3 }, "1,3", CultureInfo.InvariantCulture));
			var select = new MultiSelect("foo").Options(new[] { 1, 2, 3 });
			target.Execute(select);
			var selectedValues = ConvertToGeneric<string>(select.SelectedValues);
			select.SelectedValues.ShouldNotBeNull();
			selectedValues.ShouldCount(2);
			Assert.Contains("1", selectedValues);
			Assert.Contains("3", selectedValues);
		}

		[Test]
		public void Should_apply_model_state_values_to_multiselect_element_selected_values_when_only_one_item_is_in_model_state()
		{
			stateDictionary.SetModelValue("foo", new ValueProviderResult(1, "1", CultureInfo.InvariantCulture));
			var select = new MultiSelect("foo").Options(new[] { 1, 2, 3 });
			target.Execute(select);
			var selectedValues = ConvertToGeneric<string>(select.SelectedValues);
			select.SelectedValues.ShouldNotBeNull();
			selectedValues.ShouldCount(1);
			Assert.Contains("1", selectedValues);
		}

		private static List<T> ConvertToGeneric<T>(IEnumerable e)
		{
			var result = new List<T>();
			var enumerator = e.GetEnumerator();
			while (enumerator.MoveNext())
			{
				result.Add((T)enumerator.Current);
			}
			return result;
		}
	}
}