using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class TelephoneBoxTests
	{
		[Test]
		public void basic_telephonebox_render()
		{
			var html = new TelephoneBox("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Telephone);
		}

		[Test]
		public void telephonebox_list_renders_list()
		{
			var html = new TelephoneBox("foo").List("list1").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.List).WithValue("list1");
		}
	}
}