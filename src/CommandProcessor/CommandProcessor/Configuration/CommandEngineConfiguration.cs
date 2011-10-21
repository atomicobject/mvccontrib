using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcContrib.CommandProcessor.Configuration
{
	public class CommandEngineConfiguration
	{
		private readonly IDictionary<Type, IMessageConfiguration> _messageConfigurations =
			new Dictionary<Type, IMessageConfiguration>();

		public  IDictionary<Type, IMessageConfiguration> MessageConfigurations
		{
			get { return _messageConfigurations; }
		} 
		public IMessageConfiguration GetMessageConfiguration(Type messageType)
		{
			try
			{
				return _messageConfigurations[messageType];
			} 
			catch(KeyNotFoundException e)
			{
				var message = string.Format("Could not find message configuration for {0}.", messageType);
				throw new Exception(message, e);
			}
		}

		public void Initialize(Assembly assembly)
		{
			IEnumerable<Type> messageDefinitionTypes =
				from t in assembly.GetTypes()
				where typeof (IMessageConfiguration).IsAssignableFrom(t) && !t.IsAbstract
				select t;

			foreach (Type messageDefinitionType in messageDefinitionTypes)
			{
				if (messageDefinitionType.BaseType != null && messageDefinitionType.BaseType.IsGenericType)
				{
					Type messageType = messageDefinitionType.BaseType.GetGenericArguments()[0];
					var messageConfiguration = (IMessageConfiguration)Activator.CreateInstance(messageDefinitionType);

					_messageConfigurations.Add(messageType, messageConfiguration);
				}
			}
		}
	}
}