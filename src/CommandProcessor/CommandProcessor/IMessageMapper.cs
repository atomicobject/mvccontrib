using System;

namespace MvcContrib.CommandProcessor
{
	public interface IMessageMapper
	{
		object MapUiMessageToCommandMessage(object message, Type messageType, Type destinationType);
	}
}