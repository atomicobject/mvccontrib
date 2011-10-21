using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Html;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Html
{
    [TestFixture, Obsolete]
    public class HtmlHelperTests
    {
        private MockRepository _mocks;

        private HtmlHelper createHtmlHelper(string appPath)
        {
            var httpContext = _mocks.DynamicHttpContextBase();
            var controllerContext = new ControllerContext(httpContext, new RouteData(), _mocks.DynamicMock<ControllerBase>());
            var view = _mocks.DynamicMock<IView>();
            var viewContext = new ViewContext(controllerContext, view, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());
            var html = new HtmlHelper(viewContext, _mocks.DynamicMock<IViewDataContainer>());

            SetupResult.For(httpContext.Request.ApplicationPath).Return(appPath);

            _mocks.ReplayAll();

            return html;
        }

        [SetUp]
        public void Setup()
        {
            _mocks = new MockRepository();
        }

        [Test]
        public void ResolveUrl_returns_null_when_passed_null()
        {
            var html = createHtmlHelper(string.Empty);
            Assert.That(html.ResolveUrl(null), Is.Null);
        }

        [Test]
        public void ResolveUrl_pasess_urls_without_tilde_straight_through()
        {
            var html = createHtmlHelper("/test123");
            Assert.That(html.ResolveUrl("/some/url"), Is.EqualTo("/some/url"));
        }

        [Test]
        public void ResolveUrl_works_with_sites_in_a_virtual_directory()
        {
            var html = createHtmlHelper("/site");
            Assert.That(html.ResolveUrl("~/page3.html"), Is.EqualTo("/site/page3.html"));
        }

        [Test]
        public void ResolveUrl_works_with_sites_at_root()
        {
            var html = createHtmlHelper("");
            Assert.That(html.ResolveUrl("~/page3.html"), Is.EqualTo("/page3.html"));
        }

        [Test]
        public void Stylesheet_assumes_content_css_directory()
        {
            var html = createHtmlHelper("/site");
            var expectedTag = "<link type=\"text/css\" rel=\"stylesheet\" href=\"/site/content/css/styles.css\" />\n";

            Assert.That(html.Stylesheet("styles.css"), Is.EqualTo(expectedTag));
        }

        [Test]
        public void Stylesheet_correctly_specifies_single_media_to_css_tag()
        {
            var html = createHtmlHelper("/site");
            var expectedTag = "<link type=\"text/css\" rel=\"stylesheet\" href=\"/site/content/css/styles.css\" media=\"screen\" />\n";

            Assert.That(html.Stylesheet("styles.css", "screen"), Is.EqualTo(expectedTag)); 
        }

        [Test]
        public void Stylesheet_correctly_specifies_multiple_media_to_css_tag()
        {
            var html = createHtmlHelper("/site");
            var expectedTag = "<link type=\"text/css\" rel=\"stylesheet\" href=\"/site/content/css/styles.css\" media=\"screen, print\" />\n";

            Assert.That(html.Stylesheet("styles.css", "screen, print"), Is.EqualTo(expectedTag));
        }
        
        [Test]
        public void Stylesheet_maps_relative_paths()
        {
            var html = createHtmlHelper("/site");

            var tag = html.Stylesheet("~/other/path/styles.css");

            Assert.That( tag.Contains("/content/css"), Is.False );
        }

        [Test]
        public void ScriptInclude_assumes_content_js_directory()
        {
            var html = createHtmlHelper("/site");
            var expectedTag = "<script type=\"text/javascript\" src=\"/site/Scripts/jquery.js\" ></script>\n";

            Assert.That(html.ScriptInclude("jquery.js"), Is.EqualTo(expectedTag));
            
        }
        
        [Test]
        public void ScriptInclude_maps_relative_paths()
        {
            var html = createHtmlHelper("/site");
            var tag = html.ScriptInclude("~/other/path/doodad.js");

            Assert.That(tag.Contains("/content/js"), Is.False);
            
        }



    }
}