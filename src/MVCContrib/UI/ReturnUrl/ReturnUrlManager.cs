using System;
using System.Web;

namespace MvcContrib.UI.ReturnUrl
{
	public class ReturnUrlManager : IReturnUrlManager
	{
		public const string ParameterName = "ReturnUrl";

		public string GetReturnUrl()
		{
			var url = HttpContext.Current.Request.Params[ParameterName];

			if(string.IsNullOrEmpty(url))
			{
				throw new ApplicationException(
					string.Format(
						"The Return URL has not been set.  Check the previous page to make sure the hyperlink to this page includes {0} as a query string or form parameter.",
						ParameterName));
			}

			return url;
		}

		public bool HasReturnUrl()
		{
			var url = HttpContext.Current.Request.Params[ParameterName];
			return !string.IsNullOrEmpty(url);
		}

		public static string GetCurrentUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}
	}
}