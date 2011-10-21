using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML input element of type 'url.'
	/// </summary>
	public abstract class UrlBoxBase<T> : TextInput<T> where T : UrlBoxBase<T>
	{
		protected UrlBoxBase(string name) : base(HtmlInputType.Url, name) { }

		protected UrlBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Url, name, forMember, behaviors) { }

		/// <summary>
		/// Sets the 'list' attribute.
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		/// <returns></returns>
		public virtual T List(string value)
		{
			Attr(HtmlAttribute.List, value);
			return (T)this;
		}
	}
}