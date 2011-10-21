using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace $safeprojectname$.Area
{
	public static class HtmlHelperExtensions
	{
		public static void RenderPortableAreaSample(this HtmlHelper helper)
		{
			helper.RenderAction("Index", "Default", new { Area = "$safeprojectname$" });
		}
	}
}