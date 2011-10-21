using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base type for datetime picker.
	/// </summary>
	public abstract class DateTimePickerBase<T> : DateAndTimePickerBase<T> where T : DateTimePickerBase<T>
	{
		protected DateTimePickerBase(string name) : base(HtmlInputType.DateTime, name) { }

		protected DateTimePickerBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.DateTime, name, forMember, behaviors) { }

		protected override object FormatValue(object value)
		{
			if(value == null)
			{
				return null;
			}
			if(value is DateTime)
			{
				var format = ((DateTime)value).Kind == DateTimeKind.Unspecified
					? "{0:yyyy-MM-ddTHH:mm:ss.fffZ}"
					: "{0:yyyy-MM-ddTHH:mm:ss.fffK}";
				return string.Format(format, (DateTime)value);
			}
			return value;
		}
	}
}