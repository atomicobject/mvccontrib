using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.CommandProcessor.Helpers;
using MvcContrib.CommandProcessor.Interfaces;

namespace MvcContrib.CommandProcessor.Validation
{
	public interface IValidationEngine
	{
		ExecutionResult ValidateCommand(object commandMessage, ICommandConfiguration commandConfiguration);
	}

	public class ValidationEngine : IValidationEngine
	{
		private readonly IValidationRuleFactory _ruleFactory;

		public ValidationEngine(IValidationRuleFactory ruleFactory)
		{
			_ruleFactory = ruleFactory;
		}

		#region IValidationEngine Members

		public ExecutionResult ValidateCommand(object commandMessage, ICommandConfiguration commandConfiguration)
		{
			var totalResult = new ExecutionResult();

			IEnumerable<ValidationRuleInstance> ruleInstances = commandConfiguration.GetValidationRules();

			foreach (ValidationRuleInstance instance in ruleInstances)
			{
				if (instance.ShouldApply != null)
					if (!(bool) instance.ShouldApply.DynamicInvoke(commandMessage))
						continue;

				Delegate compile = instance.ToCheckExpression.Compile();
				object input = compile.DynamicInvoke(new object[] {commandMessage});
				bool stopProcessing = false;

				if (instance.ArrayRule)
				{
					var enumerable = (IEnumerable) input;

					int i = 0;

					foreach (object item in enumerable)
					{
						if (item == null) continue;

						IValidationRule rule = _ruleFactory.ConstructRule(instance);
						string result = rule.IsValid(item);

						bool ruleFailed = result != null;

						if (ruleFailed)
						{
							var indexedUiExpression = new IndexReplacerVisitor(i).Visit(instance.UIAttributeExpression);
							totalResult.AddMessage(result, instance.ToCheckExpression, (LambdaExpression)indexedUiExpression, instance.ToCompareExpression);

							if (rule.StopProcessing)
							{
								stopProcessing = true;
								break;
							}
						}
						i++;
					}
				}
				else
				{
					IValidationRule rule = _ruleFactory.ConstructRule(instance);

					if (rule is ICrossReferencedValidationRule)
					{
						Delegate toCompareDelegate = instance.ToCompareExpression.Compile();
						object toCompare = toCompareDelegate.DynamicInvoke(new object[] {commandMessage});
						((ICrossReferencedValidationRule) rule).ToCompare = toCompare;
					}

					string result = rule.IsValid(input);

					bool ruleFailed = result != null;

					if (ruleFailed)
					{
						totalResult.AddMessage(result, instance.ToCheckExpression, instance.UIAttributeExpression, instance.ToCompareExpression);

						if (rule.StopProcessing)
						{
							break;
						}
					}
				}
				if (stopProcessing)
					break;
			}

			return totalResult;
		}

		#endregion
	}
}