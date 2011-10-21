using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base type for week picker.
	/// </summary>
	public abstract class WeekPickerBase<T> : DateAndTimePickerBase<T> where T : WeekPickerBase<T>
	{
		private CalendarWeekRule? calendarWeekRule;
		private DayOfWeek? firstDayOfWeek;
		private object value;

		protected WeekPickerBase(string name) : base(HtmlInputType.Week, name) { }
		
		protected WeekPickerBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Week, name, forMember, behaviors) { }

		protected override object FormatValue(object value)
		{
			if(value == null)
			{
				return null;
			}
			if (value is DateTime)
			{
				var currentCulture = Thread.CurrentThread.CurrentCulture;
				calendarWeekRule = calendarWeekRule ?? currentCulture.DateTimeFormat.CalendarWeekRule;
				firstDayOfWeek = firstDayOfWeek ?? currentCulture.DateTimeFormat.FirstDayOfWeek;
				var weekNumber = currentCulture.Calendar.GetWeekOfYear((DateTime)value, calendarWeekRule.Value, firstDayOfWeek.Value);
				return string.Format("{0:yyyy}-W{1:00}", value, weekNumber);
			}
			return value;
		}

		/// <summary>
		/// Set the 'value' attribute.
		/// </summary>
		/// <param name="value">The value of the attribute.</param>
		public override T Value(object value)
		{
			this.value = value;
			return (T)this;
		}

		/// <summary>
		/// Set rules for how weeks with be evaluated
		/// </summary>
		/// <param name="calendarWeekRule">The rule to use for determining the first week of the year</param>
		/// <param name="firstDayOfWeek">The first day of the week</param>
		public T Evaluation(CalendarWeekRule calendarWeekRule, DayOfWeek firstDayOfWeek)
		{
			this.calendarWeekRule = calendarWeekRule;
			this.firstDayOfWeek = firstDayOfWeek;
			return (T)this;
		}

		protected override void PreRender()
		{
			elementValue = FormatValue(value);
			base.PreRender();
		}
	}
}