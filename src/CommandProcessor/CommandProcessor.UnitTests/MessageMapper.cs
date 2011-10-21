using System;
using AutoMapper;

namespace Tarantino.RulesEngine.CommandProcessor
{
	public class MessageMapper : IMessageMapper
	{
		private readonly IMappingEngine _mappingEngine;

		public MessageMapper(IMappingEngine mappingEngine)
		{
			_mappingEngine = mappingEngine;
		}

		public ICommandMessage MapUiMessageToCommandMessage(IMessage message, Type messageType, Type destinationType)
		{
			return (ICommandMessage) _mappingEngine.Map(message, messageType, destinationType);
		}
	}
}