namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public class ValidateUSPhoneNumber : RegexValidation<string>
	{
		public override string Example
		{
			get { return "555-123-1234"; }
		}

		protected override string Pattern
		{
			get { return "^\\s*1?[\\s\\-\\(]*\\d{3}[\\)]?[-\\s]*\\d{3}[-\\s]*\\d{4}\\s*$"; }
		}
	}
}