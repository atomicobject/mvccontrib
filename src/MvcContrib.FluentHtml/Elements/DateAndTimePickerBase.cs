using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base type for date and time picker elements.
	/// </summary>
	public abstract class DateAndTimePickerBase<T> : QuantityInputBase<T> where T : DateAndTimePickerBase<T>
	{
		protected DateAndTimePickerBase(string type, string name) : base(type, name) { }

		protected DateAndTimePickerBase(string type, string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(type, name, forMember, behaviors) { }

		/// <summary>
		/// Set the 'value' attribute.
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		public override T Value(object value)
		{
			var formattedValue = FormatValue(value);
			base.Value(formattedValue);
			return (T)this;
		}

		protected abstract object FormatValue(object value);

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

		/// <summary>
		/// Limit what input values are presented.
		/// </summary>
		/// <param name="min">The minimium value</param>
		/// <param name="max">The maximum value</param>
		/// <param name="step">The step increment</param>
		public override T Limit(object min, object max, long step)
		{
			return SetLimit(min, max, step);
		}

		/// <summary>
		/// Limit what input values are presented.
		/// </summary>
		/// <param name="min">The minimium value</param>
		/// <param name="max">The maximum value</param>
		public override T Limit(object min, object max)
		{
			return SetLimit(min, max, null);
		}

		private T SetLimit(object min, object max, long? step)
		{
			if (min != null)
			{
				Attr(HtmlAttribute.Min, FormatValue(min));
			}
			if (max != null)
			{
				Attr(HtmlAttribute.Max, FormatValue(max));
			}
			if (step != null)
			{
				Attr(HtmlAttribute.Step, step);
			}
			return (T)this;
		}
	}
}