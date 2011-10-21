using System;
using System.Linq.Expressions;
using System.Reflection;
using MvcContrib.CommandProcessor.Helpers;
using MvcContrib.CommandProcessor.Validation;

namespace MvcContrib.CommandProcessor.Configuration
{
	public class CommandConfigurationExpression<TMessage, TCommand> : ICommandConfigurationExpression<TMessage, TCommand>
	{
		private readonly CommandDefinition _CommandDefinition;

		public CommandConfigurationExpression(CommandDefinition CommandDefinition)
		{
			_CommandDefinition = CommandDefinition;
		}

		#region ICommandConfigurationExpression<TMessage,TCommand> Members

		public ICommandConfigurationExpression<TMessage, TCommand> Enforce(
			Action<IRuleExpression<TMessage, TCommand>> validation)
		{
			ValidationRuleInstance[] instances = GetValidation(validation);
			_CommandDefinition.AddValidationRules(instances);
			return this;
		}

		public ICommandConfigurationExpression<TMessage, TCommand> Enforce(
			Action<IRuleExpression<TMessage, TCommand>> validation, Action<IRuleOptionsExpression<TCommand>> options)
		{
			var validationModel = new ValidationDefinition<TMessage, TCommand>();

			validation(validationModel);

			options(validationModel);

			_CommandDefinition.AddValidationRules(validationModel.GetInstances());

			return this;
		}

		public ICommandConfigurationExpression<TMessage, TCommand> Condition(Func<TMessage, bool> condition)
		{
			_CommandDefinition.ApplyCondition(condition);

			return this;
		}

		public ICommandConfigurationExpression<TMessage, TCommand> FillFromContext(
			Expression<Func<TCommand, object>> itemToFill)
		{
			PropertyInfo propertyInfo = ReflectionHelper.FindProperty(itemToFill);
			_CommandDefinition.AddContextDependentMember(propertyInfo);
			return this;
		}

		#endregion

		private static ValidationRuleInstance[] GetValidation(Action<IRuleExpression<TMessage, TCommand>> validation)
		{
			var validationModel = new ValidationDefinition<TMessage, TCommand>();

			validation(validationModel);

			return validationModel.GetInstances();
		}
	}
}