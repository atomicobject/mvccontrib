using System;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests
{
	[TestFixture]
	public class ReturnValueTester
	{
		[Test]
		public void Can_set_value()
		{
			var expected = DateTime.Now;

			var value = new ReturnValue();
			value.SetValue(expected);

			Assert.AreEqual(typeof (DateTime), value.Type);
			Assert.AreEqual(expected, value.Value);
		}
	}
}