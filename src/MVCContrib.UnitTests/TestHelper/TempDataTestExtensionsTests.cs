using System.Web.Mvc;
using MvcContrib.TestHelper;
using NUnit.Framework;

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture]
	public class TempDataTestExtensionsTests
	{
		[Test]
		public void Does_not_throw_when_key_has_been_kept()
		{
			string key = "testkey";
			
			var tempData = new TempDataDictionary
			{
				{key, new object()}
			};

			tempData.Keep(key);
			tempData.AssertKept(key);
		}

		[Test]
		public void Throws_when_key_has_not_been_kept()
		{
			string key = "testkey";
			
			var tempData = new TempDataDictionary
			{
				{key, new object()}
			};

			tempData.Keep("nottestkey");

			Assert.Throws<MvcContrib.TestHelper.AssertionException>(
				() => tempData.AssertKept(key), "Key 'testkey' not kept.");
		}
	}
}