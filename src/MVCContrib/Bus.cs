using System;
using System.Linq;
using MvcContrib.PortableAreas;
using System.Collections.Generic;
using System.Reflection;

namespace MvcContrib
{
	public class Bus
	{
		private static IApplicationBus _instance;
		private static object _busLock = new object();

		public static IApplicationBus Instance { 
			get
			{
				InitializeTheDefaultBus();
				return _instance;
			}
			set
			{
				_instance=value;					
			}
		}

		private static void InitializeTheDefaultBus()
		{
			if(_instance==null)
			{
				lock(_busLock)
				{
					if(_instance==null)
					{
						_instance = new ApplicationBus(new MessageHandlerFactory());
						AddAllMessageHandlers();
					}
				}
			}
		}
		
		public static void Send(IEventMessage eventMessage)
		{
			Instance.Send(eventMessage);
		}

		public static void AddMessageHandler(Type type)
		{
			Instance.Add(type);
		}

		public static void AddAllMessageHandlers()
		{
			var handlers = FindAllMessageHandlers();

			foreach (var handler in handlers)
				Instance.Add(handler);
		}

		private static IEnumerable<Type> FindAllMessageHandlers()
		{
			IEnumerable<Type> allTypes = Type.EmptyTypes;

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException exception)
				{
					types = exception.Types;
				}

				allTypes = allTypes.Concat(types);
			}

			var handlerTypes = allTypes.Where(type => IsValidType(type));

			return handlerTypes;
		}

		public static bool IsValidType(Type type)
		{
			if (type == null || type.IsInterface || type.IsAbstract || type.IsNestedPrivate)
				return false;

			bool isIMessageHandler = type.GetInterface(typeof(IMessageHandler).Name) != null;

			return isIMessageHandler;
		}
	}
}