using MvcContrib.CommandProcessor.Validation.Rules;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests.Validation.Rules
{
	public class RegexValidationTester : TestBase
	{
		[Test]
		public void Should_be_successful_when_input_matches_pattern()
		{
			var rule = RegexRule<string>().ForPattern("^foo.*$").ForExample("foo bar");
			rule.IsValid("foo bar").ShouldBeSuccessful();
		}

		[Test]
		public void Should_not_be_successful_when_input_does_not_match_pattern()
		{
			var rule = RegexRule<string>().ForPattern("^foo.*$").ForExample("foo bar");
			rule.IsValid("garbage").ShouldHaveErrorMessage();
		}

		[Test]
		public void Should_have_a_reasonable_error_message()
		{
			var rule = RegexRule<string>().ForPattern("^\\d{2}\\\\.\\d{3}$").ForExample("12.123");
			rule.IsValid("garbage").ShouldHaveErrorMessage("{0} should be formatted like '12.123'");
		}

		[Test]
		public void Should_ignore_null_input()
		{
			var rule = RegexRule<int?>().ForPattern("^\\d{2}\\\\.\\d{3}$").ForExample("12.123");
			rule.IsValid(null).ShouldBeSuccessful();
		}

		[Test]
		public void Should_ignore_default_input()
		{
			var rule = RegexRule<int>().ForPattern("^\\d{2}\\\\.\\d{3}$").ForExample("12.123");
			rule.IsValid(default(int)).ShouldBeSuccessful();
		}

		[Test]
		public void Should_ignore_default_input_with_overridden_default()
		{
			var rule = RegexRuleWithDefault<int>().ForPattern("^\\d{2}\\\\.\\d{3}$").ForExample("12.123").ForDefault(123);
			rule.IsValid(123).ShouldBeSuccessful();
		}

		private static TestableRegexValidation<T> RegexRule<T>()
		{
			return new TestableRegexValidation<T>();
		}

		private static TestableRegexValidationWithDefault<T> RegexRuleWithDefault<T>()
		{
			return new TestableRegexValidationWithDefault<T>();
		}

		private class TestableRegexValidation<T> : RegexValidation<T>
		{
			private string _pattern;
			private string _example;

			public TestableRegexValidation<T> ForPattern(string pattern)
			{
				_pattern = pattern;
				return this;
			}

			public TestableRegexValidation<T> ForExample(string example)
			{
				_example = example;
				return this;
			}

			protected override string Pattern
			{
				get { return _pattern; }
			}

			public override string Example
			{
				get { return _example; }
			}
		}

		private class TestableRegexValidationWithDefault<T> : RegexValidation<T>
		{
			private string _pattern;
			private string _example;
			private object _default;

			public TestableRegexValidationWithDefault<T> ForPattern(string pattern)
			{
				_pattern = pattern;
				return this;
			}

			public TestableRegexValidationWithDefault<T> ForExample(string example)
			{
				_example = example;
				return this;
			}

			public TestableRegexValidationWithDefault<T> ForDefault(object value)
			{
				_default = value;
				return this;
			}

			protected override string Pattern
			{
				get { return _pattern; }
			}

			public override string Example
			{
				get { return _example; }
			}

			protected override object Default
			{
				get { return _default; }
			}
		}
	}
}