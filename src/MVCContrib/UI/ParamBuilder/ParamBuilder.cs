using System.Collections.Generic;
using System.Web.Routing;
using MvcContrib.UI.ReturnUrl;

namespace MvcContrib.UI.ParamBuilder
{
	public class ParamBuilder : ExplicitFacadeDictionary<string, object>
	{
		private readonly IReturnUrlManager _returnUrlManager;
		private readonly IDictionary<string, object> _param = new Dictionary<string, object>();

		public ParamBuilder(IReturnUrlManager returnUrlManager)
		{
			_returnUrlManager = returnUrlManager;
		}

		protected override IDictionary<string, object> Wrapped
		{
			get { return _param; }
		}

		public RouteValueDictionary ToRoute()
		{
			return new RouteValueDictionary(this);
		}

		public static implicit operator RouteValueDictionary(ParamBuilder paramBuilder)
		{
			return new RouteValueDictionary(paramBuilder);
		}

		public ParamBuilder Add(string key, object value)
		{
			_param.Add(key, "" + value);
			return this;
		}

		public ParamBuilder ReturnUrl()
		{
			return ReturnUrl(_returnUrlManager.GetReturnUrl());
		}

		public ParamBuilder ReturnUrl(string url)
		{
			_param.Add(ParamNames.ReturnUrl, url);
			return this;
		}
	}
}