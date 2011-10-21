using System;
using System.Collections.Generic;
using System.Reflection;
using MvcContrib.CommandProcessor.Interfaces;
using MvcContrib.CommandProcessor.Validation;

namespace MvcContrib.CommandProcessor.Configuration
{
	public class CommandDefinition : ICommandConfiguration
	{
		private readonly List<PropertyInfo> _contextDependentMembers = new List<PropertyInfo>();
		private readonly List<ValidationRuleInstance> _rules = new List<ValidationRuleInstance>();

		public CommandDefinition(Type operationType)
		{
			CommandMessageType = operationType;
			Condition = new Func<object, bool>(message => true);
		}

		#region ICommandConfiguration Members

		public Delegate Condition { get; private set; }
		public Type CommandMessageType { get; private set; }

		public void Initialize(object commandMessage, ExecutionResult result)
		{
			foreach (PropertyInfo propertyInfo in _contextDependentMembers)
			{
				object contextItem = result.ReturnItems.Get(propertyInfo.PropertyType);
				propertyInfo.SetValue(commandMessage, contextItem, new object[0]);
			}
		}

		public IEnumerable<ValidationRuleInstance> GetValidationRules()
		{
			return _rules;
		}

		#endregion

		public void AddValidationRules(IEnumerable<ValidationRuleInstance> instances)
		{
			_rules.AddRange(instances);
		}

		public void ApplyCondition<TMessage>(Func<TMessage, bool> condition)
		{
			Condition = condition;
		}

		public void AddContextDependentMember(PropertyInfo propertyInfo)
		{
			_contextDependentMembers.Add(propertyInfo);
		}
	}
}