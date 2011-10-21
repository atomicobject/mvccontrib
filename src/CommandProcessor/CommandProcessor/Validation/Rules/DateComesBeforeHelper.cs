using System;

namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public class DateComesBeforeHelper
	{
		public static string IsValid(DateTime? earlierDate, DateTime? laterDate)
		{
			DateTime nonNullEarlierDate = earlierDate ?? DateTime.MinValue;
			DateTime nonNullLaterDate = laterDate ?? DateTime.MaxValue;

			if (nonNullLaterDate.Hour == 0 && nonNullLaterDate.Minute == 0)
				nonNullLaterDate = nonNullLaterDate.AddDays(1).Subtract(new TimeSpan(0, 0, 1, 0));

			if (nonNullEarlierDate.Date == nonNullLaterDate.Date && nonNullLaterDate.Date != nonNullLaterDate &&
			    nonNullEarlierDate > nonNullLaterDate)
			{
				return "Time on {0} cannot precede time on {1}";
			}

			if (nonNullEarlierDate > nonNullLaterDate)
			{
				return "{0} cannot precede {1}";
			}

			return null;
		}
	}

	public class DateComesAfterHelper
	{
		public static string IsValid(DateTime? earlierDate, DateTime? laterDate)
		{
			string isValid = DateComesBeforeHelper.IsValid(laterDate, earlierDate);

			if (isValid != null)
				isValid = isValid.Replace("cannot precede", "cannot be after");

			return isValid;
		}
	}

	public class TimeComesBeforeHelper
	{
		public static string IsValid(DateTime? earlierDate, DateTime? laterDate)
		{
			string isValid = DateComesBeforeHelper.IsValid(earlierDate, laterDate);

			if (string.IsNullOrEmpty(isValid))
				return isValid;

			if (isValid.StartsWith("Time on"))
			{
				return "{0} cannot precede {1}";
			}

			return isValid;
		}
	}
}