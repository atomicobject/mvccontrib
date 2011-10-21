using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML input element of type 'text.'
	/// </summary>
	public abstract class TextBoxBase<T> : TextInput<TextBox> where T : TextBoxBase<T>
	{
		protected TextBoxBase(string name) : base(HtmlInputType.Text, name) {}

		protected TextBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Text, name, forMember, behaviors) {}

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