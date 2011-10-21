using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.CommandProcessor.Commands;
using MvcContrib.CommandProcessor.Configuration;
using MvcContrib.CommandProcessor.Interfaces;
using MvcContrib.CommandProcessor.Validation;
using  NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.CommandProcessor.IntegrationTests
{
	[TestFixture]
	public class MessageProcessorTester
	{
		private T S<T>() where T : class
		{
			return MockRepository.GenerateStub<T>();
		}

		[Test]
		public void The_message_processor_should_process_this_message()
		{
			//Application Startup code
			var configuration = new CommandEngineConfiguration();
			configuration.Initialize(typeof (MessageProcessorTester).Assembly);

			var locator = S<IServiceLocator>();
			locator.Stub(ioC => ioC.GetAllInstances(null)).IgnoreArguments().Return(new[] {new TestMessageCommandHandler()});
			ServiceLocator.SetLocatorProvider(() => locator);

			//request startup code.
			var invoker = new CommandInvoker(new ValidationEngine(new ValidationRuleFactory()), new CommandFactory());
			var unitOfWork = S<IUnitOfWork>();
			var mapper = S<IMessageMapper>();
			mapper.Stub(messageMapper => messageMapper.MapUiMessageToCommandMessage(null, null, null)).IgnoreArguments().Return(
				new TestCommandMessage());
			var processor = new MessageProcessor(mapper, invoker, unitOfWork, configuration);

			ExecutionResult result = processor.Process(new TestViewModel {Message = "foo"}, typeof (TestViewModel));

			Assert.True(result.Successful);
			Assert.AreEqual(0, result.Messages.Count());
			Assert.NotNull(result.ReturnItems.Get<TestCommandMessage>());
		}
	}

	public class TestViewModel
	{
		public string Message { get; set; }
	}

	public class TestMessageCommandHandler : Command<TestCommandMessage>
	{
		public bool _wasExecuted;

		protected override ReturnValue Execute(TestCommandMessage commandCommandMessage)
		{
			_wasExecuted = true;
			return new ReturnValue {Type = typeof (TestCommandMessage), Value = commandCommandMessage};
		}
	}

	public class TestCommandMessage
	{
		public string Message { get; set; }
	}

	public class Configuration : MessageDefinition<TestViewModel>
	{
		public Configuration()
		{
			Execute<TestCommandMessage>();
		}
	}
}