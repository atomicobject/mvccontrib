using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.CommandProcessor.Commands;
using MvcContrib.CommandProcessor.Configuration;
using MvcContrib.CommandProcessor.Interfaces;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests
{
	[TestFixture]
	public class RulesEngineTester
	{
		[Test]
		public void the_engine_should_create_a_message_processor()
		{
			ServiceLocator.SetLocatorProvider(() => new FakeLocator());
			var rulesEngine = new RulesEngine();
			RulesEngine.MessageProcessorFactory = new FakeFactory();

			rulesEngine.Initialize(typeof (TestMessage).Assembly, new FakeMessageMapper());
			ExecutionResult result = rulesEngine.Process(new TestMessage(), typeof (TestMessage));
			Assert.True(result.Successful);
		}

		[Test]
		public void the_engine_should_store_configuration_and_mapping_as_a_static()
		{
			var rulesEngine = new RulesEngine();
			rulesEngine.Initialize(typeof (TestMessage).Assembly, new FakeMessageMapper());
			Assert.NotNull(rulesEngine.Configuration);
		}
	}

	public class FakeMessageMapper : IMessageMapper
	{
		public object MapUiMessageToCommandMessage(object message, Type messageType, Type destinationType)
		{
			return new TestCommand();
		}
	}

	public class FakeFactory : IMessageProcessorFactory
	{
		public IMessageProcessor Create(IUnitOfWork unitOfWork, IMessageMapper mappingEngine,
		                                CommandEngineConfiguration configuration)
		{
			return new FakeProcessor();
		}
	}

	public class FakeProcessor : IMessageProcessor
	{
		public ExecutionResult Process(object message, Type messageType)
		{
			return new ExecutionResult();
		}
	}

	public class FakeLocator : IServiceLocator
	{
		#region IServiceLocator Members

		public object GetService(Type serviceType)
		{
			throw new NotImplementedException();
		}

		public object GetInstance(Type serviceType)
		{
			throw new NotImplementedException();
		}

		public object GetInstance(Type serviceType, string key)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return new List<object>();
		}

		public TService GetInstance<TService>()
		{
			return (TService) ((object) new FakeUoW());
		}

		public TService GetInstance<TService>(string key)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TService> GetAllInstances<TService>()
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	public class FakeUoW : IUnitOfWork
	{
		public void Dispose() {}

		public void Begin() {}

		public void Invalidate() {}

		public void Commit() {}
	}

	public class TestMessage : object //Defined in UI project?
	{}

	public class TestMessageConfiguration : MessageDefinition<TestMessage>
	{
		public TestMessageConfiguration()
		{
			Execute<TestCommand>();
		}
	}

	public class TestCommand : object //object defined in Core
	{}

	public class TestCommandHandler : Command<TestCommand>
	{
		protected override ReturnValue Execute(TestCommand commandMessage)
		{
			throw new NotImplementedException();
		}
	}
}