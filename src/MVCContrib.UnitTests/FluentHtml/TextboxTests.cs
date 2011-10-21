using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using MvcContrib.UnitTests.FluentHtml.CustomBehaviors;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Html;
using RangeAttribute=System.ComponentModel.DataAnnotations.RangeAttribute;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class TextBoxTests
	{
		[Test]
		public void basic_textbox_renders_with_id_and_name_and_value()
		{
			var html = new TextBox("foo.Bar").ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveAttributesCount(4)
				.ShouldBeNamed(HtmlTag.Input);

			element.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Text);
			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
			element.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("");
		}

		[Test]
		public void model_textbox_renders_with_id_and_name()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString();
			html.ShouldHaveHtmlNode("Person_FirstName")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Person.FirstName");
		}

		[Test]
		public void model_textbox_renders_with_id_and_name_from_nested_member()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString();
			html.ShouldHaveHtmlNode("Person_FirstName").ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Person.FirstName");
		}

		[Test]
		public void textbox_id_renders_id()
		{
			new TextBox("foo.Bar").Id("some_id").ToString()
				.ShouldHaveHtmlNode("some_id");
		}

		[Test]
		public void textbox_value_renders_value()
		{
			new TextBox("x").Value("some value").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("some value");
		}

		[Test]
		public void textbox_format_renders_formatted_value()
		{
			var item = 100.00m;
			var expected = string.Format("{0:$0.00}", item);
			new TextBox("x").Value(item).Format("$0.00").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Value).WithValue(expected);
		}

		[Test]
		public void textbox_format_using_full_format_placeholder_renders_formatted_value()
		{
			var item = 100.00m;
			var expected = string.Format("{0:$0.00}", item);
			new TextBox("x").Value(item).Format("{0:$0.00}").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Value).WithValue(expected);
		}

		[Test]
		public void textbox_maxlength_renders_maxlength()
		{
			new TextBox("x").MaxLength(100).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.MaxLength).WithValue("100");
		}

		[Test]
		public void textbox_title_renders_title()
		{
			new TextBox("x").Title("Click here").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Title).WithValue("Click here");
		}

		[Test]
		public void textbox_size_renders_size()
		{
			new TextBox("x").Size(22).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Size).WithValue("22");
		}

		[Test]
		public void textbox_styles_renders_style()
		{
			var html = new TextBox("x").Styles(color => "#000", text_align => "right").ToString();

			html.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Style)
				.WithValue("color:#000;text-align:right;");
		}

		[Test]
		public void textbox_onclick_renders_onclick()
		{
			new TextBox("x").OnClick("return false;").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlEventAttribute.OnClick).WithValue("return false;");
		}

		[Test]
		public void textbox_class_renders_class()
		{
			new TextBox("x").Class("required").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Class).WithValue("required");
		}

		[Test]
		public void textbox_multiple_classes_renders_compound_class()
		{
			new TextBox("x").Class("required").Class("date").ToString()
				.ShouldHaveHtmlNode("x").ShouldHaveAttribute(HtmlAttribute.Class)
				.ValueShouldContain("required")
				.ValueShouldContain("date");
		}

		[Test]
		public void textbox_disabled_renders_disabled()
		{
			new TextBox("x").Disabled(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Disabled).WithValue(HtmlAttribute.Disabled);
		}

		[Test]
		public void textbox_with_label_renders_label()
		{
			var label = new TextBox("x").Label("Foo:").ToString()
				.ShouldRenderHtmlDocument().ChildNodes[0];
			label.ShouldHaveAttribute(HtmlAttribute.Id).WithValue("x_Label");
			label.ShouldHaveAttribute(HtmlAttribute.For).WithValue("x");
			label.ShouldHaveInnerTextEqual("Foo:");
		}

		[Test]
		public void textbox_with_label_renders_label_after()
		{
			var element = new TextBox("x").LabelAfter("Foo:").ToString()
				.ShouldRenderHtmlDocument().ChildNodes[1];
			element.ShouldHaveAttribute(HtmlAttribute.Id).WithValue("x_Label");
			element.ShouldHaveAttribute(HtmlAttribute.For).WithValue("x");
			element.ShouldHaveInnerTextEqual("Foo:");
		}

		[Test]
		public void textbox_not_disabled_renders_not_disabled()
		{
			new TextBox("x").Disabled(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.Disabled);
		}

		[Test]
		public void text_box_for_member_with_maxlength_attibute_sets_maxlength()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Title;
			var behaviors = new List<IBehaviorMarker> { new CustomMaxLengthBehavior() };
			var expectedLength = expression.GetMemberExpression().GetAttribute<RangeAttribute>().Maximum;

			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();

			var element = html.ShouldHaveHtmlNode("Title");
			element.ShouldHaveAttribute(HtmlAttribute.MaxLength).WithValue(expectedLength.ToString());
		}

		[Test]
		public void text_box_for_member_with_required_attibute_adds_set_class_using_default_behavior()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Id;
			var behaviors = new List<IBehaviorMarker> { new CustomRequiredHtmlBehavior() };

			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();

			var element = html.ShouldHaveHtmlNode("Id");
			element.ShouldHaveAttribute(HtmlAttribute.Class).ValueShouldContain("req");
		}

		[Test]
		public void text_box_for_member_with_required_attibute_adds_set_class_using_custom_behavior()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Id;
			var behaviors = new List<IBehaviorMarker> { new CustomRequiredHtmlBehavior() };

			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();

			var element = html.ShouldHaveHtmlNode("Id");
			element.ShouldHaveAttribute(HtmlAttribute.Class).ValueShouldContain("req");
		}

        [Test]
        public void text_box_for_member_with_required_attibute_adds_required_attribute_using_custom_behavior()
        {
            Expression<Func<FakeModel, object>> expression = x => x.Id;
            var behaviors = new List<IBehaviorMarker> { new CustomRequiredHtml5Behavior() };

            var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();

            var element = html.ShouldHaveHtmlNode("Id");
            element.ShouldHaveAttribute(HtmlAttribute.Required).ValueShouldContain("required");
        }

        [Test]
        public void text_box_for_member_with_expression_attribute_adds_pattern_attribute_using_custom_behavior()
        {
            Expression<Func<FakeModel, object>> expression = x => x.Telephone;
            var behaviors = new List<IBehaviorMarker> {new CustomExpressionHtmlBehavior()};

            var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();

            var element = html.ShouldHaveHtmlNode("telephone");
            element.ShouldHaveAttribute(HtmlAttribute.Pattern).ValueShouldContain(@"\(\d\d\d\) \d\d\d\-\d\d\d\d");
        }

        [Test]
        public void text_box_for_member_with_range_attribute_adds_min_and_max_attributes_using_custom_behavior()
        {
            Expression<Func<FakeModel, object>> expression = x => x.Quantity;
            var behaviors = new List<IBehaviorMarker> { new CustomRangeHtmlBehavior() };

            var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();

            var element = html.ShouldHaveHtmlNode("quantity");
            element.ShouldHaveAttribute(HtmlAttribute.Min).ValueShouldContain("1");
            element.ShouldHaveAttribute(HtmlAttribute.Max).ValueShouldContain("10");
        }

	    public void text_box_for_member_with_required_attribute_adds_metadata_to_class_using_custom_behavior()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Id;
			var behaviors = new List<IBehaviorMarker> { new CustomRequiredInMetadataBehavior(0), new AppyJsonMetadataToCssBehavior(1) };

			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();

			var element = html.ShouldHaveHtmlNode("Id");
			var expectedMetadata = HttpUtility.HtmlEncode("{'required':true}");
			element.ShouldHaveAttribute(HtmlAttribute.Class).ValueShouldContain(expectedMetadata);
		}

		[Test]
		public void text_box_for_member_with_required_attribute_and_additional_css_classes_adds_metadata_to_class_using_custom_behavior()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Id;
			var behaviors = new List<IBehaviorMarker> { new CustomRequiredInMetadataBehavior(0), new AppyJsonMetadataToCssBehavior(1) };

			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).Class("MyOtherClass").ToString();

			var element = html.ShouldHaveHtmlNode("Id");
			var expectedMetadata = HttpUtility.HtmlEncode("{'required':true}");
			element.ShouldHaveAttribute(HtmlAttribute.Class).ValueShouldContain(expectedMetadata);
			element.ShouldHaveAttribute(HtmlAttribute.Class).ValueShouldContain("MyOtherClass");
		}

		[Test]
		public void text_box_for_member_with_multiple_attributes_populates_metadata_in_css_class_from_custom_behaviors()
		{
			Expression<Func<FakeModel, object>> expression = x => x.MultiAttributedProperty;
			var behaviors = new List<IBehaviorMarker> { new AppyJsonMetadataToCssBehavior(1), new CustomRequiredInMetadataBehavior(0), new CustomMaxLengthInMetadataBehavior(0) };
			var html = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors).ToString();
			var element = html.ShouldHaveHtmlNode("MultiAttributedProperty");
			var expectedMetadata = HttpUtility.HtmlEncode("{'required':true,'maximum':50,'minimum':0}");
			element.ShouldHaveAttribute(HtmlAttribute.Class).ValueShouldContain(expectedMetadata);
		}

		[Test]
		public void textbox_with_label_class_renders_label_with_class()
		{
			var label = new TextBox("foo.Bar").Label("Foo:", "bar").ToString()
				.ShouldRenderHtmlDocument().ChildNodes[0];

			label.ShouldHaveAttribute(HtmlAttribute.Class).WithValue("bar");
		}

		[Test]
		public void text_box_value_html_attribute_encodes_value()
		{
			var value = "<div>Foo</div>";
			new TextBox("x").Value(value).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Value).WithValue(HttpUtility.HtmlAttributeEncode(value));
		}

		[Test]
		public void textbox_autocomplete_true_renders_autocomplete_on()
		{
			new TextBox("x").Autocomplete(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Autocomplete).WithValue("on");
		}

		[Test]
		public void textbox_autocomplete_false_renders_autocomplete_off()
		{
			new TextBox("x").Autocomplete(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Autocomplete).WithValue("off");
		}

		[Test]
		public void textbox_autofocus_true_renders_autofocus()
		{
			new TextBox("x").Autofocus(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Autofocus).WithValue(HtmlAttribute.Autofocus);
		}
		
		[Test]
		public void textbox_autofocus_false_does_not_render_autofocus()
		{
			new TextBox("x").Autofocus(true).Autofocus(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.Autofocus);
		}

		[Test]
		public void textbox_novalidate_true_renders_novalidate()
		{
			new TextBox("x").Novalidate(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.NoValidate).WithValue(HtmlAttribute.NoValidate);
		}

		[Test]
		public void textbox_novalidate_false_does_not_render_novalidate()
		{
			new TextBox("x").Novalidate(true).Novalidate(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.NoValidate);
		}
		
		[Test]
		public void textbox_pattern_renders_pattern()
		{
			new TextBox("x").Pattern("test").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Pattern).WithValue("test");
		}

		[Test]
		public void textbox_placeholder_renders_placeholder()
		{
			new TextBox("x").Placeholder("test").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Placeholder).WithValue("test");
		}
		
		[Test]
		public void textbox_required_true_renders_required()
		{
			new TextBox("x").Required(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Required).WithValue(HtmlAttribute.Required);
		}

		[Test]
		public void textbox_required_false_does_not_render_required()
		{
			new TextBox("x").Required(true).Required(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.Required);
		}

		[Test]
		public void textbox_forms_renders_forms()
		{
			new TextBox("x").Form("form1", "form2").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Form).WithValue("form1 form2");
		}

		[Test]
		public void textbox_list_renders_list()
		{
			var html = new TextBox("foo").List("list1").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.List).WithValue("list1");
		}
	}
}
