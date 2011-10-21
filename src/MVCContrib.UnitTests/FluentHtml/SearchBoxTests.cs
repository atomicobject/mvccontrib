using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class SearchBoxTests
	{
		[Test]
		public void basic_searchbox_render()
		{
			var html = new SearchBox("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Search);
		}
		
		[Test]
		public void searchbox_list_renders_list()
		{
			var html = new SearchBox("foo").List("list1").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.List).WithValue("list1");
		}
	}
}