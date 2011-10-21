using System;
using MvcContrib.CommandProcessor.Interfaces;

namespace MvcContrib.CommandProcessor.Helpers
{
	internal static class Converter
	{
		public static DateTime? ToDateTime(string date, string hour, string minute)
		{
			DateTime? dateTime = null;

			DateTime? parsedDate = Parse(date);
			int? parsedHour = ParseInteger(hour, 0, 23);
			int? parsedMinute = ParseInteger(minute, 0, 59);

			bool hasDate = parsedDate != null;
			bool hasTime = parsedHour != null && parsedMinute != null;

			if (hasDate && hasTime)
			{
				return new DateTime(parsedDate.Value.Year, parsedDate.Value.Month, parsedDate.Value.Day, parsedHour.Value,
				                    parsedMinute.Value, 0);
			}

			if (hasDate)
			{
				return new DateTime(parsedDate.Value.Year, parsedDate.Value.Month, parsedDate.Value.Day, 0, 0, 0);
			}

			return dateTime;
		}

		public static int ToInt32(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return default(int);
			}
			return Convert.ToInt32(value);
		}

		public static DateTime? ToDateTime(object date)
		{
			DateTime dateTime;
			if (!DateTime.TryParse((string) date, out dateTime))
				return null;
			return dateTime;
		}

		public static DateTime? ToDateTime(IDateAndTime dateAndTime)
		{
			if (dateAndTime == null)
				return null;

			if (dateAndTime.IsEmpty())
				return null;

			return ToDateTime(dateAndTime.Date, dateAndTime.Hour, dateAndTime.Minute);
		}

		private static DateTime? Parse(string date)
		{
			DateTime dateTime;
			bool parseSucceeded = DateTime.TryParse(date, out dateTime);
			return parseSucceeded ? (DateTime?) dateTime : null;
		}

		public static int? ParseInteger(string integerString)
		{
			int integer;
			bool parseSucceeded = int.TryParse(integerString, out integer);
			return parseSucceeded ? (int?) integer : null;
		}

		private static int? ParseInteger(string value, int min, int max)
		{
			int? intValue = ParseInteger(value);
			if (intValue < min || intValue > max)
				return null;
			return intValue;
		}
	}
}