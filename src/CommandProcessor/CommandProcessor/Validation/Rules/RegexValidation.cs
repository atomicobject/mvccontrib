using System.Text.RegularExpressions;

namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public abstract class RegexValidation<T> : ValidationRule<T>
	{
		public string Message { get { return string.Format("{{0}} should be formatted like '{0}'", Example); } }
		public abstract string Example { get; }
		protected abstract string Pattern { get; }
		protected virtual object Default { get { return default(T); } }

		protected override string IsValidCore(T input)
		{
			if (Equals(input, Default))
				return Success();

			if (Regex.IsMatch(input.ToString(), Pattern))
				return Success();

			return Message;
		}
	}
}