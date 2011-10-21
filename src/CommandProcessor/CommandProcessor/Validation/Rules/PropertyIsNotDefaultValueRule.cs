namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public abstract class PropertyIsNotDefaultValueRule<T> : ValidationRule<T>
	{
		private const string MessageFormat = "{0} is a required field";
		protected abstract string Label { get; }

		protected override string IsValidCore(T input)
		{
			return NotDefault(input) ? Success() : GetMessage();
		}

		private static bool NotDefault(T input)
		{
			return !Equals(default(T), input);
		}

		public string GetMessage()
		{
			return string.Format(MessageFormat, Label);
		}
	}
}