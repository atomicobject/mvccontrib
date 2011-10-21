using System;
using System.Web;

namespace Tarantino.RulesEngine.Mvc
{
	public interface IWebContext
	{
		string GetRequestItem(string key);
	}

	public class WebContext : IWebContext
	{
		public string GetRequestItem(string key)
		{
			var value = HttpContext.Current.Request[key];
			return value;
		}
	}
}