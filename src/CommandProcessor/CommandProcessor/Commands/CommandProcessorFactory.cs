using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.CommandProcessor.Interfaces;

namespace MvcContrib.CommandProcessor.Commands
{
	public interface ICommandFactory
	{
		IEnumerable<ICommandMessageHandler> GetCommands(ICommandConfiguration definition);
	}

	public class CommandFactory : ICommandFactory
	{
		private static readonly Type _genericHandler = typeof (Command<>);

		#region ICommandFactory Members

		public IEnumerable<ICommandMessageHandler> GetCommands(ICommandConfiguration definition)
		{
			Type concreteCommandType = _genericHandler.MakeGenericType(definition.CommandMessageType);
			IEnumerable<ICommandMessageHandler> commands =
				ServiceLocator.Current.GetAllInstances(concreteCommandType).Cast<ICommandMessageHandler>();
			return commands;
		}

		#endregion
	}
}