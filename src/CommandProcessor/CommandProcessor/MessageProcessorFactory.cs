using MvcContrib.CommandProcessor.Commands;
using MvcContrib.CommandProcessor.Configuration;
using MvcContrib.CommandProcessor.Interfaces;
using MvcContrib.CommandProcessor.Validation;

namespace MvcContrib.CommandProcessor
{
	public class MessageProcessorFactory : IMessageProcessorFactory
	{
		public IMessageProcessor Create(IUnitOfWork unitOfWork, IMessageMapper mappingEngine,
		                                CommandEngineConfiguration configuration)
		{
			return new MessageProcessor(mappingEngine,
			                            new CommandInvoker(new ValidationEngine(new ValidationRuleFactory()),
			                                               new CommandFactory()), unitOfWork, configuration);
		}
	}
}