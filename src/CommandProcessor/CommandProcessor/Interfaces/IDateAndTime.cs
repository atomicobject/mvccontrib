using System;

namespace MvcContrib.CommandProcessor.Interfaces
{
	public interface IDateAndTime
	{
		string Date { get; set; }
		string Hour { get; set; }
		string Minute { get; set; }
		bool IsEmpty();
		DateTime? GetValue();
	}
}