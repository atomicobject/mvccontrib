using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate an HTML input element of type 'color.'
	/// </summary>
	public class ColorPickerBase<T> : Input<T> where T : ColorPickerBase<T>
	{
		public ColorPickerBase(string name) : base(HtmlInputType.Color, name) { }

		public ColorPickerBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Color, name, forMember, behaviors) { }
		
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