using System;

namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public abstract class DateSequenceRule<T> : ValidationRule<T>
	{
		public override bool StopProcessing
		{
			get { return false; }
		}

		protected override string IsValidCore(T input)
		{
			DateTime? earlierDate = GetEarlierDate(input);
			DateTime? laterDate = GetLaterDate(input);

			DateTime nonNullEarlierDate = earlierDate ?? DateTime.MinValue;
			DateTime nonNullLaterDate = laterDate ?? DateTime.MaxValue;

			if (nonNullLaterDate.Hour == 0 && nonNullLaterDate.Minute == 0)
				nonNullLaterDate = nonNullLaterDate.AddDays(1).Subtract(new TimeSpan(0, 0, 1, 0));

			if (nonNullEarlierDate > nonNullLaterDate)
			{
				string earlierDateLabel = GetEarlierDateLabel();
				string laterDateLabel = GetLaterDateLabel();

				return string.Format("{0} cannot precede {1}", laterDateLabel, earlierDateLabel);
			}

			return Success();
		}

		protected abstract string GetLaterDateLabel();
		protected abstract string GetEarlierDateLabel();
		protected abstract DateTime? GetLaterDate(T input);
		protected abstract DateTime? GetEarlierDate(T input);
	}

	public abstract class TimeSequenceRule<T> : DateSequenceRule<T>
	{
		protected override string IsValidCore(T input)
		{
			DateTime? earlierDate = GetEarlierDate(input);
			DateTime? laterDate = GetLaterDate(input);
			if (!earlierDate.HasValue || !laterDate.HasValue)
				return Success();
			if (earlierDate.Value.Date != laterDate.Value.Date)
				return Success();
			return base.IsValidCore(input);
		}
	}
}