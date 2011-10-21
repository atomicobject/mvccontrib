using MvcContrib.CommandProcessor.Validation.Rules;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests.Validation.Rules
{
	public class ValidateUSPhoneNumberTester : TestBase
	{
		[Test]
		public void Should_allow_phone_numbers_in_various_formats()
		{
			var rule = new ValidateUSPhoneNumber();

			rule.IsValid("5129998888").ShouldBeSuccessful();
			rule.IsValid("512-999-8888").ShouldBeSuccessful();
			rule.IsValid("512 999 8888").ShouldBeSuccessful();
			rule.IsValid("(512) 999 8888").ShouldBeSuccessful();
			rule.IsValid("1-512-999-8888").ShouldBeSuccessful();
			rule.IsValid("1 512 999 8888").ShouldBeSuccessful();
			rule.IsValid(" 1  512  999 - 8888  ").ShouldBeSuccessful();
		}

		[Test]
		public void Should_not_allow_missing_digits()
		{
			var rule = new ValidateUSPhoneNumber();
			rule.IsValid("999-8888").ShouldHaveErrorMessage();
		}

		[Test]
		public void Should_not_allow_extra_digits()
		{
			var rule = new ValidateUSPhoneNumber();
			rule.IsValid("512-999-12345").ShouldHaveErrorMessage();
		}

		[Test]
		public void Should_have_a_good_example()
		{
			var rule = new ValidateUSPhoneNumber();
			rule.IsValid(rule.Example).ShouldBeSuccessful();
		}
	}
}