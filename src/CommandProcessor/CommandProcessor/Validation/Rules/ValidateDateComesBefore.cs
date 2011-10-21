using System;

namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public class ValidateDateComesBefore : AbstractCrossReferenceValidationRule<DateTime?>
	{
		public override bool StopProcessing
		{
			get { return false; }
		}

		protected override string IsValidCore(DateTime? toCheck, DateTime? toCompare)
		{
			string validationResult = DateComesBeforeHelper.IsValid(toCheck, toCompare);
			if (string.IsNullOrEmpty(validationResult))
			{
				return Success();
			}

			return validationResult;
		}
	}

	public class ValidateDateComesAfter : AbstractCrossReferenceValidationRule<DateTime?>
	{
		public override bool StopProcessing
		{
			get { return false; }
		}

		protected override string IsValidCore(DateTime? toCheck, DateTime? toCompare)
		{
			string validationResult = DateComesAfterHelper.IsValid(toCheck, toCompare);
			if (string.IsNullOrEmpty(validationResult))
			{
				return Success();
			}

			return validationResult;
		}
	}

	public class ValidateTimeComesBefore : AbstractCrossReferenceValidationRule<DateTime?>
	{
		public override bool StopProcessing
		{
			get { return false; }
		}

		protected override string IsValidCore(DateTime? toCheck, DateTime? toCompare)
		{
			string validationResult = TimeComesBeforeHelper.IsValid(toCheck, toCompare);
			if (string.IsNullOrEmpty(validationResult))
			{
				return Success();
			}

			return validationResult;
		}
	}
}