using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base type for date picker.
	/// </summary>
	public abstract class DatePickerBase<T> : DateAndTimePickerBase<T> where T : DatePickerBase<T>
	{
		protected DatePickerBase(string name) : base(HtmlInputType.Date, name) { }

		protected DatePickerBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Date, name, forMember, behaviors) { }

		protected override object FormatValue(object value)
		{
			return value == null
				? null
				: (value is DateTime
					? string.Format("{0:yyyy-MM-dd}", value)
					: value);
		}
	}
}