using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class RangeBoxTests
	{
		[Test]
		public void basic_ranagebox_render()
		{
			var html = new RangeBox("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Range);
		}

		[Test]
		public void rangebox_list_renders_list()
		{
			var html = new RangeBox("foo").List("list1").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.List).WithValue("list1");
		}

		[Test]
		public void rangebox_novalidate_true_renders_novalidate()
		{
			new RangeBox("x").Novalidate(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.NoValidate).WithValue(HtmlAttribute.NoValidate);
		}

		[Test]
		public void rangebox_novalidate_false_does_not_render_novalidate()
		{
			new RangeBox("x").Novalidate(true).Novalidate(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.NoValidate);
		}

		[Test]
		public void rangebox_required_true_renders_required()
		{
			new RangeBox("x").Required(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Required).WithValue(HtmlAttribute.Required);
		}

		[Test]
		public void rangebox_required_false_does_not_render_required()
		{
			new RangeBox("x").Required(true).Required(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.Required);
		}
	}
}