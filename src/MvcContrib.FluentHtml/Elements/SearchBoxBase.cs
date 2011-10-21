using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML input element of type 'search.'
	/// </summary>
	public abstract class SearchBoxBase<T> : TextInput<T> where T : SearchBoxBase<T>
	{
		protected SearchBoxBase(string name) : base(HtmlInputType.Search, name) {}

		protected SearchBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Search, name, forMember, behaviors) {}

		/// <summary>
		/// Sets the 'list' attribute.
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		public virtual T List(string value)
		{
			Attr(HtmlAttribute.List, value);
			return (T)this;
		}
	}
}