using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class FileUploadTests
	{
		[Test]
		public void basic_file_upload_renders_with_corect_tag_and_type()
		{
			new FileUpload("x").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.File);
		}

		[Test]
		public void file_upload_with_multiple_renders_multiple()
		{
			var html = new FileUpload("foo").Multiple(true).ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.Multiple).WithValue(HtmlAttribute.Multiple);
		}
		
		[Test]
		public void fileupload_with_multiple_renders_multiple()
		{
			var html = new FileUpload("foo").Multiple(true).ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.Multiple).WithValue(HtmlAttribute.Multiple);
		}

		[Test]
		public void fileupload_without_multiple_does_not_render_multiple()
		{
			var html = new FileUpload("foo").Multiple(true).Multiple(false).ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldNotHaveAttribute(HtmlAttribute.Multiple);
		}
		
		[Test]
		public void fileupload_with_maxlength_renders_maxlength()
		{
			var html = new FileUpload("foo").MaxLength(13).ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldHaveAttribute(HtmlAttribute.MaxLength).WithValue("13");
		}
	}
}
