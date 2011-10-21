using System;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.CommandProcessor.Configuration;
using MvcContrib.CommandProcessor.Interfaces;

namespace MvcContrib.CommandProcessor
{
	public interface IRulesEngine
	{
		ExecutionResult Process(object message, Type messageType);
		ExecutionResult Process(object message);
	}

	public class RulesEngine : IRulesEngine
	{
		private static CommandEngineConfiguration _configuration;
		public static IMessageProcessorFactory MessageProcessorFactory = new MessageProcessorFactory();
		public static object _lock = new object();

		private static IMessageMapper _mappingEngine;

		public CommandEngineConfiguration Configuration
		{
			get { return _configuration; }
			private set { _configuration = value; }
		}

		public IMessageMapper MappingEngine
		{
			get { return _mappingEngine; }
			private set { _mappingEngine = value; }
		}

		public void Initialize(Assembly assembly, IMessageMapper messageMapper)
		{
			if (_mappingEngine == null)
			{
				lock (_lock)
				{
					if (_configuration == null)
					{
						Configuration = new CommandEngineConfiguration();
						Configuration.Initialize(assembly);
						_mappingEngine = messageMapper;
					}
				}
			}
		}

		private IServiceLocator locator
		{
			get
			{
				try
				{
					return ServiceLocator.Current;
				}
				catch (Exception)
				{
					throw new InvalidOperationException(
						"The Microsoft.Practices.ServiceLocation.ServiceLocator must be initialized by calling the SetLocatorProvider method.");
				}
			}
		}

		public ExecutionResult Process(object message, Type messageType)
		{
			var unitOfWork = locator.GetInstance<IUnitOfWork>();
			IMessageProcessor processor = MessageProcessorFactory.Create(unitOfWork, MappingEngine, Configuration);
			return processor.Process(message, messageType);
		}

		public ExecutionResult Process(object message)
		{
			return Process(message, message.GetType());
		}
	}
}