using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Tarantino.RulesEngine.Mvc
{
	public static class HtmlExtensions
	{
		public static string OriginalForm(this HtmlHelper helper)
		{
			return OriginalForm(helper, helper.ViewData.Model);
		}

		public static string OriginalForm(this HtmlHelper helper, object originalFormInstance)
		{
			string formContent =
				helper.ViewContext.HttpContext.Request.Params[OriginalFormRetriever.ORIGINAL_FORM_KEY] ??
				new Serializer(new Base64Utility()).Serialize(originalFormInstance);

			return helper.Hidden(OriginalFormRetriever.ORIGINAL_FORM_KEY, formContent);
		}
	}
}