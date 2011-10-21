using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class NumberBoxTests
	{
		[Test]
		public void basic_numberbox_render()
		{
			var html = new NumberBox("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Number);
		}

		[Test]
		public void numberbox_list_renders_list()
		{
			var html = new NumberBox("foo").List("list1").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.List).WithValue("list1");
		}
		
		[Test]
		public void numberbox_required_true_renders_required()
		{
			new NumberBox("x").Required(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Required).WithValue(HtmlAttribute.Required);
		}

		[Test]
		public void numberbox_required_false_does_not_render_required()
		{
			new NumberBox("x").Required(true).Required(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.Required);
		}

		[Test]
		public void numberbox_limit_sets_limits()
		{
			var element = new NumberBox("x").Limit(0, 50, 5).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("0");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("50");
			element.ShouldHaveAttribute(HtmlAttribute.Step).WithValue("5");
		}

		[Test]
		public void numberbox_limit_sets_limits_without_step()
		{
			var element = new NumberBox("x").Limit(0, 50).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("0");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("50");
			element.ShouldNotHaveAttribute(HtmlAttribute.Step);
		}
	}
}