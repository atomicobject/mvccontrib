using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base type for date time local picker.
	/// </summary>
	public abstract class DateTimeLocalPickerBase<T> : DateAndTimePickerBase<T> where T : DateTimeLocalPickerBase<T>
	{
		protected DateTimeLocalPickerBase(string name) : base(HtmlInputType.DateTimeLocal, name) {}

		protected DateTimeLocalPickerBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.DateTimeLocal, name, forMember, behaviors) { }

		protected override object FormatValue(object value)
		{
			return value == null
				? null
				: (value is DateTime
					? string.Format("{0:yyyy-MM-ddTHH:mm:ss.fff}", value)
					: value);
		}
	}
}