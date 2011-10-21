using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ActionResults;

namespace MvcContrib.Filters
{
	/// <summary>
	/// When placed on a controller or action, this attribute will ensure
	/// that reference-type-parameters (or value types which cannot be converted from a string) passed into 
	/// RedirectToAction&lt;T&gt;() will get passed to the controller or action that this attribute is placed on.
	/// </summary>
	public class PassParametersDuringRedirectAttribute : ActionFilterAttribute
	{
		public const string RedirectParameterPrefix = "__RedirectParameter__";

		/// <summary>
		/// Loads parameters from TempData into the ActionParameters dictionary.
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			LoadParameterValuesFromTempData(filterContext);
		}

		/// <summary>
		/// Stores any parameters passed to the generic RedirectToAction method in TempData.
		/// </summary>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var result = filterContext.Result as IControllerExpressionContainer;
			var redirectResult = filterContext.Result as RedirectToRouteResult;

			if(result != null && result.Expression != null && redirectResult != null)
			{
				var storedParameters = AddParameterValuesFromExpressionToTempData(filterContext.Controller.TempData,
				                                                                  result.Expression);
				RemoveStoredParametersFromRouteValues(redirectResult.RouteValues, storedParameters.Keys);
			}
		}


		// Copied this method from Microsoft.Web.Mvc.dll (MVC Futures)...
		// Microsoft.Web.Mvc.Internal.ExpresisonHelper.AddParameterValuesFromExpressionToDictionary().
		// The only change I made is saving the parameter values to TempData instead
		// of a RouteValueDictionary.
		private static IDictionary<string, object> AddParameterValuesFromExpressionToTempData(TempDataDictionary tempData,
		                                                                                      MethodCallExpression call)
		{
			var parameters = call.Method.GetParameters();
			var parsedParameters = new Dictionary<string, object>();

			if(parameters.Length > 0)
			{
				for(var i = 0; i < parameters.Length; i++)
				{
					var expression = call.Arguments[i];
					object parameterValue = null;
					var constantExpression = expression as ConstantExpression;
					if(constantExpression != null)
					{
						parameterValue = constantExpression.Value;
					}
					else
					{
						var lambdaExpression =
							Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)), new ParameterExpression[0]);
						parameterValue = lambdaExpression.Compile()();
					}

					//Only store reference types and value types which cannot be converted to from strings.
					//In other words, if it can go in the query-string, then it should, if not, then we use temp-data
					if(parameterValue != null &&
					   !(parameterValue is string) &&
					   (!(parameterValue is ValueType) ||
					    !(TypeDescriptor.GetConverter(parameterValue).CanConvertFrom(typeof(string)))))
					{
						tempData[RedirectParameterPrefix + parameters[i].Name] = parameterValue;
						parsedParameters.Add(parameters[i].Name, parameterValue);
					}
				}
			}

			return parsedParameters;
		}

		private static void LoadParameterValuesFromTempData(ActionExecutingContext filterContext)
		{
			var actionParameters = filterContext.ActionDescriptor.GetParameters();

			foreach(var storedParameter in GetStoredParameterValues(filterContext))
			{
				if(storedParameter.Value == null)
				{
					continue;
				}

				var storedParameterName = GetParameterName(storedParameter.Key);

				if(actionParameters.Any(actionParameter => actionParameter.ParameterName == storedParameterName &&
				                                           actionParameter.ParameterType.IsAssignableFrom(
				                                           	storedParameter.Value.GetType())))
				{
					filterContext.ActionParameters[storedParameterName] = storedParameter.Value;

					filterContext.Controller.TempData.Keep(storedParameter.Key);
				}
			}
		}

		private static void RemoveStoredParametersFromRouteValues(RouteValueDictionary dictionary,
		                                                          IEnumerable<string> keysToRemove)
		{
			foreach(var key in keysToRemove)
			{
				dictionary.Remove(key);
			}
		}

		private static string GetParameterName(string key)
		{
			if(key.StartsWith(RedirectParameterPrefix))
			{
				return key.Substring(RedirectParameterPrefix.Length);
			}
			return key;
		}

		private static IList<KeyValuePair<string, object>> GetStoredParameterValues(ActionExecutingContext filterContext)
		{
			return filterContext.Controller.TempData.Where(td => td.Key.StartsWith(RedirectParameterPrefix)).ToList();
		}
	}
}