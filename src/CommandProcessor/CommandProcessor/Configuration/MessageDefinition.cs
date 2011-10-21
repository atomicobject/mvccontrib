using System.Collections.Generic;
using System.Linq;
using MvcContrib.CommandProcessor.Interfaces;

namespace MvcContrib.CommandProcessor.Configuration
{
	public interface IMessageConfiguration
	{
		IEnumerable<ICommandConfiguration> GetApplicableCommands(object message);
	}

	public abstract class MessageDefinition<TMessage> : IMessageConfiguration
	{
		public const int INDEX = int.MaxValue;
		private static readonly ConditionExpression _conditions = new ConditionExpression();

		private readonly List<CommandDefinition> _CommandDefinitions = new List<CommandDefinition>();

		protected IConditionExpression<TMessage> Conditions
		{
			get { return _conditions; }
		}

		#region IMessageConfiguration Members

		IEnumerable<ICommandConfiguration> IMessageConfiguration.GetApplicableCommands(object message)
		{
			return _CommandDefinitions
				.Where(op => (bool) op.Condition.DynamicInvoke(new[] {message}))
				.Cast<ICommandConfiguration>();
		}

		#endregion

		public ICommandConfigurationExpression<TMessage, TCommand> Execute<TCommand>()
		{
			var CommandConfiguration = new CommandDefinition(typeof (TCommand));

			_CommandDefinitions.Add(CommandConfiguration);

			return new CommandConfigurationExpression<TMessage, TCommand>(CommandConfiguration);
		}

		#region Nested type: ConditionExpression

		private class ConditionExpression : IConditionExpression<TMessage> {}

		#endregion
	}

	public interface IConditionExpression<TMessage> {}
}