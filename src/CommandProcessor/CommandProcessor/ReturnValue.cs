using System;

namespace MvcContrib.CommandProcessor
{
	public class ReturnValue
	{
		public Type Type { get; set; }
		public object Value { get; set; }

		public ReturnValue SetValue<T>(T input)
		{
			Type = typeof (T);
			Value = input;
			return this;
		}
	}
}