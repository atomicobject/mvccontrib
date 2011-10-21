using System.Collections.Generic;
using MvcContrib.CommandProcessor.Interfaces;
using MvcContrib.CommandProcessor.Validation;

namespace MvcContrib.CommandProcessor.Commands
{
	public interface ICommandInvoker
	{
		ExecutionResult Process(object commandMessage, ICommandConfiguration commandConfiguration);
	}

	public class CommandInvoker : ICommandInvoker
	{
		private readonly ICommandFactory _handerFactory;
		private readonly IValidationEngine _validationEngine;

		public CommandInvoker(IValidationEngine validationEngine, ICommandFactory handerFactory)
		{
			_validationEngine = validationEngine;
			_handerFactory = handerFactory;
		}

		#region ICommandInvoker Members

		public ExecutionResult Process(object commandMessage, ICommandConfiguration commandConfiguration)
		{
			ExecutionResult executionResult = _validationEngine.ValidateCommand(commandMessage, commandConfiguration);

			if (executionResult.Successful)
			{
				IEnumerable<ICommandMessageHandler> handlers = _handerFactory.GetCommands(commandConfiguration);

				foreach (ICommandMessageHandler handler in handlers)
				{
					ReturnValue returnObject = handler.Execute(commandMessage);

					executionResult.Merge(returnObject);
				}
			}

			return executionResult;
		}

		#endregion
	}
}