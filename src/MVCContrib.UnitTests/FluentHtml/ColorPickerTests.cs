using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ColorPickerTests
	{
		[Test]
		public void basic_colorpicker_render()
		{
			var html = new ColorPicker("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Color);
		}
		
		[Test]
		public void colorbox_list_renders_list()
		{
			var html = new ColorPicker("foo").List("list1").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.List).WithValue("list1");
		}

		[Test]
		public void colorpicker_novalidate_true_renders_novalidate()
		{
			new ColorPicker("x").Novalidate(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.NoValidate).WithValue(HtmlAttribute.NoValidate);
		}

		[Test]
		public void colorpicker_novalidate_false_does_not_render_novalidate()
		{
			new ColorPicker("x").Novalidate(true).Novalidate(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.NoValidate);
		}
	}
}