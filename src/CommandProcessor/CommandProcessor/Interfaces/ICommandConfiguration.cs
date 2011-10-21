using System;
using System.Collections.Generic;
using MvcContrib.CommandProcessor.Validation;

namespace MvcContrib.CommandProcessor.Interfaces
{
	public interface ICommandConfiguration
	{
		Type CommandMessageType { get; }
		Delegate Condition { get; }
		void Initialize(object commandMessage, ExecutionResult result);
		IEnumerable<ValidationRuleInstance> GetValidationRules();
	}
}