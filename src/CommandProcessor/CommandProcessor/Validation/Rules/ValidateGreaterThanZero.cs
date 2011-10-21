namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public class ValidateGreaterThanZero : ValidationRule<int?>
	{
		public const string ErrorMessage = "{0} must be a positive number.";

		protected override string IsValidCore(int? input)
		{
			if (!input.HasValue || input > 0) return null;

			return ErrorMessage;
		}
	}
}