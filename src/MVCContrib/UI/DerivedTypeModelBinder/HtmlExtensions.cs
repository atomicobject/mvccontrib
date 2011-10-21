using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcContrib.Binders;

namespace MvcContrib.UI.DerivedTypeModelBinder
{
	public static class HtmlExtensions
	{
		/// <summary>
		/// Renders metadata used by the derived type model binder to determine
		/// the concrete type to instantiate.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <returns>a hidden field rendering that holds the type metadata</returns>
		public static MvcHtmlString TypeStamp<TModel>(this HtmlHelper<TModel> htmlHelper)
		{
			return htmlHelper.Hidden(DerivedTypeModelBinderCache.TypeStampFieldName, DerivedTypeModelBinderCache.GetTypeName(typeof(TModel)));
		}
	}
}
