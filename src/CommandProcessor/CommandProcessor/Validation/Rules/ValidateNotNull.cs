using System;

namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public class ValidateRequired : ValidateNotNull
	{
		public static readonly string Error = "{0} is a required field";

		protected override string GetErrorMessage()
		{
			return Error;
		}
	}

	public class ValidateDateRequired : ValidateRequired
	{
		protected override string IsValidCore(object toCheck)
		{
			var date = (DateTime) toCheck;
			if (date == DateTime.MinValue)
				return base.IsValidCore(null);
			return base.IsValidCore(toCheck);
		}
	}

	public class ValidateNotNull : ValidationRule<object>
	{
		public override bool StopProcessing
		{
			get { return true; }
		}

		protected override string IsValidCore(object toCheck)
		{
			if (toCheck == null)
				return GetErrorMessage();

			var stringToCheck = toCheck as string;

			if (stringToCheck == string.Empty)
				return GetErrorMessage();

			return Success();
		}

		protected virtual string GetErrorMessage()
		{
			return "{0} is missing or invalid";
		}
	}
}