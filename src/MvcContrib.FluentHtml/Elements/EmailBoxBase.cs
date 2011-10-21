using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class EmailBoxBase<T> : TextInput<T> where T : EmailBoxBase<T> 
	{
		protected EmailBoxBase(string name) : base(HtmlInputType.Email, name) {}
		
		protected EmailBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Email, name, forMember, behaviors) { }

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

		/// <summary>
		/// Add or remove the 'multiple' attribute. 
		/// </summary>
		/// <param name="value">Whether to add or remove the attribute.</param>
		public virtual T Multiple(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Multiple, HtmlAttribute.Multiple);
			}
			else
			{
				((IElement)this).RemoveAttr(HtmlAttribute.Multiple);
			}
			return (T)this;
		}
	}
}