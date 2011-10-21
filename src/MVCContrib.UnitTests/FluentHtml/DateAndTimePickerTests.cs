using System;
using System.Globalization;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class DateAndTimePickerTests
	{
		[Test]
		public void basic_datepicker_render()
		{
			var html = new DatePicker("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Date);
		}

		[Test]
		public void datepicker_correctly_formats_date_value()
		{
			var value = new DateTime(2000, 2, 2, 14, 5, 24, 331);
			var element = new DatePicker("test").Value(value);
			element.ValueAttributeShouldEqual("2000-02-02");
		}

		[Test]
		public void datepicker_correctly_render_null_value()
		{
			var element = new DatePicker("test").Value(null);
			element.ValueAttributeShouldEqual(null);
		}

		[Test]
		public void datepicker_limit_sets_limits()
		{
			var element = new DatePicker("x").Limit(new DateTime(2000, 1, 1), new DateTime(2000, 12, 31), 7).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("2000-01-01");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("2000-12-31");
			element.ShouldHaveAttribute(HtmlAttribute.Step).WithValue("7");
		}

		[Test]
		public void datepicker_novalidate_true_renders_novalidate()
		{
			new DatePicker("x").Novalidate(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.NoValidate).WithValue(HtmlAttribute.NoValidate);
		}

		[Test]
		public void datepicker_novalidate_false_does_not_render_novalidate()
		{
			new DatePicker("x").Novalidate(true).Novalidate(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.NoValidate);
		}
		
		[Test]
		public void datepicker_required_true_renders_required()
		{
			new DatePicker("x").Required(true).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Required).WithValue(HtmlAttribute.Required);
		}

		[Test]
		public void datepicker_required_false_does_not_render_required()
		{
			new DatePicker("x").Required(true).Required(false).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldNotHaveAttribute(HtmlAttribute.Required);
		}

		[Test]
		public void basic_timepicker_render()
		{
			var html = new TimePicker("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Time);
		}

		[Test]
		public void timepicker_correctly_formats_date_value()
		{
			var value = new DateTime(2000, 2, 2, 14, 5, 24, 331);
			var element = new TimePicker("test").Value(value);
			element.ValueAttributeShouldEqual("14:05:24.331");
		}

		[Test]
		public void timepicker_limit_sets_limits()
		{
			var element = new DatePicker("x").Limit(new TimeSpan(0, 1, 0, 0), new TimeSpan(10, 20, 30), 2).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("01:00:00");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("10:20:30");
			element.ShouldHaveAttribute(HtmlAttribute.Step).WithValue("2");
		}

		[Test]
		public void timepicker_correctly_formats_timespan_value()
		{
			var value = new TimeSpan(0, 14, 5, 24, 331);
			var element = new TimePicker("test").Value(value);
			element.ValueAttributeShouldEqual("14:05:24.331");
		}

		[Test]
		public void basic_datetimepicker_render()
		{
			var html = new DateTimePicker("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.DateTime);
		}

		[Test]
		public void datetimepicker_correctly_formats_utc_date_value()
		{
			var value = new DateTime(2000, 2, 2, 14, 5, 24, 331, DateTimeKind.Utc);
			var element = new DateTimePicker("test").Value(value);
			element.ValueAttributeShouldEqual("2000-02-02T14:05:24.331Z");
		}
		
		[Test]
		public void datetimepicker_correctly_formats_unspecified_date_value()
		{
			var value = new DateTime(2000, 2, 2, 14, 5, 24, 331);
			var element = new DateTimePicker("test").Value(value);
			element.ValueAttributeShouldEqual("2000-02-02T14:05:24.331Z");
		}

		[Test]
		public void datetimepicker_correctly_formats_local_date_value()
		{
			var value = new DateTime(2000, 2, 2, 14, 5, 24, 331, DateTimeKind.Local);
			var element = new DateTimePicker("test").Value(value);
			var timeZoneOffset = string.Format("{0:xK}", value).Substring(1, 6); //Note: 'K' by itself blows up.
			var expectedValue = string.Format("2000-02-02T14:05:24.331{0}", timeZoneOffset);
			element.ValueAttributeShouldEqual(expectedValue);
		}

		[Test]
		public void datetimepicker_limit_sets_limits()
		{
			var element = new DateTimePicker("x").Limit(new DateTime(2000, 1, 1), new DateTime(2000, 12, 31, 10, 20, 30, 331), 3).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("2000-01-01T00:00:00.000Z");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("2000-12-31T10:20:30.331Z");
			element.ShouldHaveAttribute(HtmlAttribute.Step).WithValue("3");
		}

		[Test]
		public void basic_monthpicker_render()
		{
			var html = new MonthPicker("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Month);
		}

		[Test]
		public void monthpicker_correctly_formats_date_value()
		{
			var value = new DateTime(2000, 2, 2);
			var element = new MonthPicker("test").Value(value);
			element.ValueAttributeShouldEqual("2000-02");
		}

		[Test]
		public void monthpicker_limit_sets_limits()
		{
			var element = new MonthPicker("x").Limit(new DateTime(2000, 1, 1), new DateTime(2000, 12, 31, 10, 20, 30, 331), 3).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("2000-01");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("2000-12");
			element.ShouldHaveAttribute(HtmlAttribute.Step).WithValue("3");
		}

		[Test]
		public void basic_datetimelocal_picker_render()
		{
			var html = new DateTimeLocalPicker("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.DateTimeLocal);
		}

		[Test]
		public void datetimelocalpicker_correctly_formats_date_value()
		{
			var value = new DateTime(2000, 2, 2, 14, 33, 31, 331);
			var element = new DateTimeLocalPicker("test").Value(value);
			element.ValueAttributeShouldEqual("2000-02-02T14:33:31.331");
		}

		[Test]
		public void datetimepickerlocal_limit_sets_limits()
		{
			var element = new DateTimeLocalPicker("x").Limit(new DateTime(2000, 1, 1), new DateTime(2000, 12, 31), 3).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("2000-01-01T00:00:00.000");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("2000-12-31T00:00:00.000");
			element.ShouldHaveAttribute(HtmlAttribute.Step).WithValue("3");
		}

		[Test]
		public void basic_weekpicker_render()
		{
			var html = new WeekPicker("foo").ToString();
			html.ShouldHaveHtmlNode("foo")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Week);
		}

		[Test]
		public void weekpicker_correctly_formats_date_value()
		{
			var value = new DateTime(2011, 1, 3);
			var element = new WeekPicker("test").Value(value);
			element.ValueAttributeShouldEqual("2011-W02");
		}

		[Test]
		public void weekpicker_correctly_formats_date_value_using_custom_rules()
		{
			var value = new DateTime(2011, 1, 2);
			var element = new WeekPicker("test")
				.Evaluation(CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday)
				.Value(value);
			element.ValueAttributeShouldEqual("2011-W01");
		}

		[Test]
		public void weekpicker_limit_sets_limits()
		{
			var element = new WeekPicker("x")
				.Evaluation(CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday)
				.Limit(new DateTime(2000, 1, 2), new DateTime(2000, 01, 9), 3).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("2000-W01");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("2000-W02");
			element.ShouldHaveAttribute(HtmlAttribute.Step).WithValue("3");
		}

		[Test]
		public void weekpicker_limit_sets_limits_without_step()
		{
			var element = new WeekPicker("x")
				.Evaluation(CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday)
				.Limit(new DateTime(2000, 1, 2), new DateTime(2000, 01, 9)).ToString()
				.ShouldHaveHtmlNode("x");
			element.ShouldHaveAttribute(HtmlAttribute.Min).WithValue("2000-W01");
			element.ShouldHaveAttribute(HtmlAttribute.Max).WithValue("2000-W02");
			element.ShouldNotHaveAttribute(HtmlAttribute.Step);
		}
	}
}