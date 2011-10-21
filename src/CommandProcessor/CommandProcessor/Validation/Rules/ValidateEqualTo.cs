namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public class ValidateEqualTo : AbstractCrossReferenceValidationRule<object>
	{
		protected virtual string ErrorMessage
		{
			get { return "{0} must be the same as {1}"; }
		}

		protected override string IsValidCore(object toCheck, object toCompare)
		{
			if (toCheck == null) return Success();

			if (! toCheck.Equals(toCompare))
			{
				return ErrorMessage;
			}

			return Success();
		}
	}
}