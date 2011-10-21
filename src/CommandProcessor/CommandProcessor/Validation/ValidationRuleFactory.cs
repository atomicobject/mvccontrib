using Microsoft.Practices.ServiceLocation;

namespace MvcContrib.CommandProcessor.Validation
{
	public interface IValidationRuleFactory
	{
		IValidationRule ConstructRule(ValidationRuleInstance ruleInstance);
	}

	public class ValidationRuleFactory : IValidationRuleFactory
	{
		#region IValidationRuleFactory Members

		public IValidationRule ConstructRule(ValidationRuleInstance ruleInstance)
		{
			return (IValidationRule)
			       ServiceLocator.Current.GetInstance(ruleInstance.ValidationRuleType);
		}

		#endregion
	}
}