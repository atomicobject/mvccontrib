using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML input element of type 'tel.'
	/// </summary>
	public abstract class TelephoneBoxBase<T> : TextInput<T> where T : TelephoneBoxBase<T>
	{
		protected TelephoneBoxBase(string name) : base(HtmlInputType.Telephone, name) {}

		protected TelephoneBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Telephone, name, forMember, behaviors) {}

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