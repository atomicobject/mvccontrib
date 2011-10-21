using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.TestHelper
{
	public static class TempDataTestExtensions
	{
		/// <summary>
		/// Asserts that a key has been passed to TempData.Keep(key)
		/// </summary>
		/// <param name="TempData">TempData collection</param>
		/// <param name="key">The key to assert kept.</param>
		public static void AssertKept(this TempDataDictionary TempData, string key)
		{
			var keptKeysField = typeof(TempDataDictionary).GetField("_retainedKeys", BindingFlags.NonPublic | BindingFlags.Instance);

			var keptKeys = keptKeysField.GetValue(TempData) as HashSet<string>;

			if(!keptKeys.Contains(key))
			{
				throw new AssertionException(String.Format("Key '{0}' not kept.", key));
			}
		}
	}
}