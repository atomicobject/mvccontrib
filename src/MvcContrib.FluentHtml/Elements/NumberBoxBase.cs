using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class NumberBoxBase<T> : QuantityInputBase<T> where T : NumberBoxBase<T>
	{
		protected NumberBoxBase(string name) : base(HtmlInputType.Number, name) { }

		protected NumberBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) 
			: base(HtmlInputType.Number, name, forMember, behaviors) {}

		/// <summary>
		/// Sets the 'list' attribute.
		/// </summary>
		/// <param name="value">The value of the attribute</param>
		public virtual T List(string value)
		{
			Attr(HtmlAttribute.List, value);
			return (T)this;
		}

		/// <summary>
		/// Add or remove the 'required' attribute. 
		/// </summary>
		/// <param name="value">Whether to add or remove the attribute.</param>
		public virtual T Required(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Required, HtmlAttribute.Required);
			}
			else
			{
				((IElement)this).RemoveAttr(HtmlAttribute.Required);
			}
			return (T)this;
		}
	}
}