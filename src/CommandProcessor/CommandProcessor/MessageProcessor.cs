using System;
using System.Collections.Generic;
using MvcContrib.CommandProcessor.Commands;
using MvcContrib.CommandProcessor.Configuration;
using MvcContrib.CommandProcessor.Interfaces;

namespace MvcContrib.CommandProcessor
{
	public interface IMessageProcessor
	{
		ExecutionResult Process(object message, Type messageType);
	}

	public class MessageProcessor : IMessageProcessor
	{
		private readonly ICommandInvoker _commandInvoker;
		private readonly CommandEngineConfiguration _configuration;
		private readonly IMessageMapper _mappingEngine;
		private readonly IUnitOfWork _unitOfWork;

		public MessageProcessor(IMessageMapper mappingEngine, ICommandInvoker commandInvoker, IUnitOfWork unitOfWork,
		                        CommandEngineConfiguration configuration
			)
		{
			_mappingEngine = mappingEngine;
			_commandInvoker = commandInvoker;
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		}

		#region IMessageProcessor Members

		public ExecutionResult Process(object message, Type messageType)
		{
			var totalResult = new ExecutionResult();
			IMessageConfiguration messageConfiguration = _configuration.GetMessageConfiguration(messageType);

			IEnumerable<ICommandConfiguration> commandConfigurations = messageConfiguration.GetApplicableCommands(message);

			foreach (ICommandConfiguration commandConfiguration in commandConfigurations)
			{
				object commandMessage =
					_mappingEngine.MapUiMessageToCommandMessage(message, messageType, commandConfiguration.CommandMessageType);

				commandConfiguration.Initialize(commandMessage, totalResult);

				ExecutionResult results = _commandInvoker.Process(commandMessage, commandConfiguration);
				totalResult.MergeWith(results);

				if (!totalResult.Successful)
				{
					_unitOfWork.Invalidate();
					break;
				}
			}

			return totalResult;
		}

		#endregion
	}
}