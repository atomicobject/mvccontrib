using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace MvcContrib
{
	/// <summary>
	/// Extension methods for UrlHelper
	/// </summary>
	public static class UrlHelperExtensions
	{
		public static string Action<TController>(this UrlHelper urlHelper, Expression<Action<TController>> expression) where TController : Controller
		{
			return LinkBuilder.BuildUrlFromExpression(urlHelper.RequestContext, urlHelper.RouteCollection, expression);
		}

		public static string Resource(this UrlHelper urlHelper, string resourceName)
		{
			var areaName = (string)urlHelper.RequestContext.RouteData.DataTokens["area"];
			return urlHelper.Action("Index", "EmbeddedResource", new {resourceName, area = areaName});
		}
	}
}