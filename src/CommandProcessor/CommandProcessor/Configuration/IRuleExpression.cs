using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.CommandProcessor.Interfaces;
using MvcContrib.CommandProcessor.Validation;

namespace MvcContrib.CommandProcessor.Configuration
{
	public interface IRuleExpression<TMessage, TCommandMessage> : IGrammar
	{
		ITargetTypeExpression<TMessage, TCommandMessage> Rule<T>() where T : IValidationRule;

		ITargetTypeExpression<TMessage, TCommandMessage> Rule<T>(Expression<Func<TCommandMessage, object>> toCheck)
			where T : IValidationRule;

		ITargetTypeExpression<TMessage, TCommandMessage> Rule<T>(Expression<Func<TCommandMessage, object>> toCheck,
		                                                         Expression<Func<TCommandMessage, object>> toCompare)
			where T : ICrossReferencedValidationRule;
	}

	public interface ICommandConfigurationExpression<TMessage, TCommandMessage> : IGrammar
	{
		ICommandConfigurationExpression<TMessage, TCommandMessage> Enforce(
			Action<IRuleExpression<TMessage, TCommandMessage>> validation);

		ICommandConfigurationExpression<TMessage, TCommandMessage> Enforce(
			Action<IRuleExpression<TMessage, TCommandMessage>> validation,
			Action<IRuleOptionsExpression<TCommandMessage>> options);

		ICommandConfigurationExpression<TMessage, TCommandMessage> Condition(Func<TMessage, bool> condition);

		ICommandConfigurationExpression<TMessage, TCommandMessage> FillFromContext(
			Expression<Func<TCommandMessage, object>> itemToFill);
	}

	public interface ITargetTypeExpression<TMessage, TCommandMessage> : IResultExpression<TMessage>
	{
		ITypedTargetExpression<TMessage, TCommandMessage, TTarget> ForType<TTarget>();
	}

	public interface ITypedTargetExpression<TMessage, TCommandMessage, TTarget> : IResultExpression<TMessage>
	{
		IResultExpression<TMessage> Targets(Expression<Func<TCommandMessage, TTarget>> property);
		IResultExpression<TMessage> Targets(Expression<Func<TCommandMessage, IEnumerable<TTarget>>> property);
	}

	public interface ITargetExpression<TCommandMessage, TMessage> : IGrammar
	{
		IResultExpression<TMessage> Targets<TTarget>(Expression<Func<TCommandMessage, TTarget>> property);
	}

	public interface IResultExpression<TMessage> : IGrammar
	{
		void RefersTo(Expression<Func<TMessage, object>> uiProperty);
	}

	public interface IRuleOptionsExpression<TCommandMessage> : IGrammar
	{
		void Condition(Predicate<TCommandMessage> condition);
	}
}