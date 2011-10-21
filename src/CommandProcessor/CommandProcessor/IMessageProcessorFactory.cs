using MvcContrib.CommandProcessor.Configuration;
using MvcContrib.CommandProcessor.Interfaces;

namespace MvcContrib.CommandProcessor
{
	public interface IMessageProcessorFactory
	{
		IMessageProcessor Create(IUnitOfWork unitOfWork, IMessageMapper mappingEngine, CommandEngineConfiguration configuration);
	}
}