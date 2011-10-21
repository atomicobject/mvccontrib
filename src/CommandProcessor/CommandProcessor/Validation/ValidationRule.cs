namespace MvcContrib.CommandProcessor.Validation
{
	public abstract class ValidationRule<T> : IValidationRule
	{
		#region IValidationRule Members

		public virtual string IsValid(object input)
		{
			return IsValidCore((T) input);
		}

		public virtual bool StopProcessing
		{
			get { return false; }
		}

		#endregion

		protected abstract string IsValidCore(T input);

		public static string Success()
		{
			return ValidationRuleResults.Success;
		}
	}

	public class ValidationRuleResults
	{
		public static readonly string Success;
	}
}