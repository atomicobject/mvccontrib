using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for range box.
	/// </summary>
	public abstract class RangeBoxBase<T> : QuantityInputBase<T> where T : RangeBoxBase<T>
	{
		protected RangeBoxBase(string name) : base(HtmlInputType.Range, name) { }

		protected RangeBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Range, name, forMember, behaviors) { }

		/// <summary>
		/// Sets the 'list' attribute.
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
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

		/// <summary>
		/// Add or remove the 'novalidate' attribute. 
		/// </summary>
		/// <param name="value">Whether to add or remove the attribute.</param>
		public virtual T Novalidate(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.NoValidate, HtmlAttribute.NoValidate);
			}
			else
			{
				((IElement)this).RemoveAttr(HtmlAttribute.NoValidate);
			}
			return (T)this;
		}
	}
}