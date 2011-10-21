using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base type for month picker.
	/// </summary>
	public abstract class MonthPickerBase<T> : DateAndTimePickerBase<T> where T : MonthPickerBase<T>
	{
		protected MonthPickerBase(string name) : base(HtmlInputType.Month, name) { }

		protected MonthPickerBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Month, name, forMember, behaviors) {}

		protected override object FormatValue(object value)
		{
			return value == null
				? null
				: (value is DateTime
					? string.Format("{0:yyyy-MM}", value)
					: value);
		}
	}
}