using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class SubmitButtonTests
	{
		[Test]
		public void submit_button_renders_with_corect_tag_and_type()
		{
			new SubmitButton("x").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Submit);
		}

		[Test]
		public void submit_button_with_action_renders_action()
		{
			var html = new SubmitButton("foo").Action("test").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormAction).WithValue("test");
		}

		[Test]
		public void submit_button_with_form_renders_form()
		{
			var html = new SubmitButton("foo").Form("test").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.Form).WithValue("test");
		}

		[Test]
		public void submit_button_with_enctype_renders_enctype()
		{
			var html = new SubmitButton("foo").EncType("test").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormEncType).WithValue("test");
		}

		[Test]
		public void submit_button_with_method_renders_method()
		{
			var html = new SubmitButton("foo").Method(FormMethod.Post).ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormMethod).WithValue("post");
		}

		[Test]
		public void submit_button_with_target_renders_target()
		{
			var html = new SubmitButton("foo").Target("test").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormTarget).WithValue("test");
		}

		[Test]
		public void submit_button_with_target_blank_renders_target_blank()
		{
			var html = new SubmitButton("foo").TargetBlank().ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormTarget).WithValue("_blank");
		}

		[Test]
		public void submit_button_with_target_parent_renders_target_parent()
		{
			var html = new SubmitButton("foo").TargetParent().ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormTarget).WithValue("_parent");
		}

		[Test]
		public void submit_button_with_target_top_renders_target_top()
		{
			var html = new SubmitButton("foo").TargetTop().ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormTarget).WithValue("_top");
		}

		[Test]
		public void submit_button_with_target_self_renders_target_self()
		{
			var html = new SubmitButton("foo").TargetSelf().ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.FormTarget).WithValue("_self");
		}

        [Test]
        public void submit_button_with_formnovalidate_attr_renders_attribute()
        {
            var html = new SubmitButton("foo").Attr("formnovalidate", "formnovalidate").ToString();

            html.ShouldHaveHtmlNode("foo").ShouldHaveAttribute("formnovalidate").WithValue("formnovalidate");
        }

        [Test]
        public void submit_button_with_formnovalidate_renders_attribute()
        {
            var html = new SubmitButton("foo").FormNoValidate(true).ToString();

            html.ShouldHaveHtmlNode("foo").ShouldHaveAttribute(HtmlAttribute.FormNoValidate).WithValue("formnovalidate");
        }
	}
}
