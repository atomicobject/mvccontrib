using MvcContrib.CommandProcessor.Validation;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests.Validation
{
	public static class ValidationTestExtenstions
	{
		public static void ShouldBeSuccessful(this string value)
		{
			if (value == ValidationRule<string>.Success())
				return;
			Assert.Fail("Validation should have been successful");
		}

		public static void ShouldHaveErrorMessage(this string value)
		{
			if (value == ValidationRule<string>.Success())
				Assert.Fail("Validation result should contain an error message");
		}

		public static void ShouldHaveErrorMessage(this string value, string expected)
		{
			Assert.AreEqual(expected, value);
		}
	}
}