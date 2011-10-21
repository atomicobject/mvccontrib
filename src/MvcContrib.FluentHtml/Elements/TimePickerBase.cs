using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base type for time picker.
	/// </summary>
	public abstract class TimePickerBase<T> : DateAndTimePickerBase<T> where T : TimePickerBase<T>
	{
		protected TimePickerBase(string name) : base(HtmlInputType.Time, name) {}

		protected TimePickerBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Time, name, forMember, behaviors) {}

		protected override object FormatValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			TimeSpan timeValue;
			if (value is TimeSpan)
			{
				timeValue = (TimeSpan)value;
			}
			else if (value is DateTime)
			{
				timeValue = ((DateTime)value).TimeOfDay;
			}
			else
			{
				return value;
			}
			return string.Format("{0:00}:{1:00}:{2:00}.{3}",
				timeValue.Hours, timeValue.Minutes, timeValue.Seconds, timeValue.Milliseconds);
		}
	}
}