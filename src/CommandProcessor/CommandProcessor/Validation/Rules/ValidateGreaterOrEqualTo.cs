using System;

namespace MvcContrib.CommandProcessor.Validation.Rules
{
	public class ValidateGreaterOrEqualTo : AbstractCrossReferenceValidationRule<IComparable>
	{
		private const string Message = "{0} should be greater than or equal to {1}";

		protected override string IsValidCore(IComparable toCheck, IComparable toCompare)
		{
			return toCompare.CompareTo(toCheck) < 0 ? Message : null;
		}
	}
}