using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for text input elements.
	/// </summary>
	/// <typeparam name="T">The derived type</typeparam>
	public abstract class TextInput<T> : Input<T>, ISupportsMaxLength where T : TextInput<T>
	{
		protected string _format;

		protected TextInput(string type, string name) : base(type, name) { }

		protected TextInput(string type, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(type, name, forMember, behaviors) { }

		/// <summary>
		/// Set the 'maxlength' attribute. 
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		public virtual T MaxLength(int value)
		{
			Attr(HtmlAttribute.MaxLength, value);
			return (T)this;
		}

		/// <summary>
		/// Specify a format string to be applied to the value.  The format string can be either a
		/// specification (e.g., '$#,##0.00') or a placeholder (e.g., '{0:$#,##0.00}').
		/// </summary>
		/// <param name="value">A format string.</param>
		public T Format(string value)
		{
			_format = value;
			return (T)this;
		}

		/// <summary>
		/// Set the 'autocomplete' attribute to 'on' or 'off.' 
		/// </summary>
		/// <param name="value">Whether to set the attribute to 'off' or 'on.'</param>
		public virtual T Autocomplete(bool value)
		{
			Attr(HtmlAttribute.Autocomplete, value ? "on" : "off");
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

		/// <summary>
		/// Set the 'pattern' attribute. 
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		public T Pattern(string value)
		{
			Attr(HtmlAttribute.Pattern, value);
			return (T)this;
		}

		/// <summary>
		/// Set the 'placeholder' attribute. 
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		public T Placeholder(string value)
		{
			Attr(HtmlAttribute.Placeholder, value);
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

		protected override void PreRender()
		{
			FormatValue();
			base.PreRender();
		}

		protected virtual void FormatValue()
		{
			if (!string.IsNullOrEmpty(_format))
			{
				if (_format.StartsWith("{0") && _format.EndsWith("}"))
				{
					elementValue = string.Format(_format, elementValue);
				}
				else
				{
					elementValue = string.Format("{0:" + _format + "}", elementValue);
				}
			}
		}
	}
}
