using System;
using MvcContrib.CommandProcessor.Validation.Rules;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests
{
	[TestFixture]
	public class DateSequenceRuleTester
	{
		public class TestDateSequenceRule : DateSequenceRule<TestOperation>
		{
			protected override string GetLaterDateLabel()
			{
				return "Earlier Date";
			}

			protected override string GetEarlierDateLabel()
			{
				return "Later Date";
			}

			protected override DateTime? GetLaterDate(TestOperation input)
			{
				return input.LaterDate;
			}

			protected override DateTime? GetEarlierDate(TestOperation input)
			{
				return input.EarlierDate;
			}
		}

		public class TestOperation
		{
			public DateTime? EarlierDate { get; set; }
			public DateTime? LaterDate { get; set; }
		}

		[Test]
		public void
			Should_determine_first_date_is_after_later_date_when_first_date_has_time_and_later_date_is_the_previous_day()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 8, 30, 0),
				LaterDate = new DateTime(2008, 4, 14, 0, 0, 0)
			};

			string result = rule.IsValid(operation);
			Assert.NotNull(result);
		}

		[Test]
		public void
			Should_determine_first_date_is_after_later_date_when_later_date_has_time_and_later_date_is_the_previous_day()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 0, 0, 0),
				LaterDate = new DateTime(2008, 4, 14, 8, 30, 0)
			};

			string result = rule.IsValid(operation);
			Assert.NotNull(result);
		}

		[Test]
		public void Should_determine_first_date_is_before_later_date_when_first_date_has_time_and_dates_are_equal()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 8, 30, 0),
				LaterDate = new DateTime(2008, 4, 15, 0, 0, 0)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_determine_first_date_is_before_later_date_when_first_date_has_time_and_later_date_is_the_next_day()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 8, 30, 0),
				LaterDate = new DateTime(2008, 4, 16, 0, 0, 0)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_determine_first_date_is_before_later_date_when_later_date_has_time_and_dates_are_equal()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 0, 0, 0),
				LaterDate = new DateTime(2008, 4, 15, 8, 30, 0)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_determine_first_date_is_before_later_date_when_later_date_has_time_and_later_date_is_the_next_day()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 0, 0, 0),
				LaterDate = new DateTime(2008, 4, 16, 8, 30, 0)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_determine_two_dates_with_time_are_out_of_order()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 9, 30, 0),
				LaterDate = new DateTime(2008, 4, 15, 8, 30, 0)
			};

			string result = rule.IsValid(operation);
			Assert.NotNull(result);
		}

		[Test]
		public void Should_determine_two_dates_without_time_are_out_of_order()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 16),
				LaterDate = new DateTime(2008, 4, 15)
			};

			string result = rule.IsValid(operation);
			Assert.NotNull(result);
		}

		[Test]
		public void Should_not_return_error_if_both_dates_are_undefined()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = null,
				LaterDate = null
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_not_return_error_if_earlier_date_is_undefined()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = null,
				LaterDate = new DateTime(2008, 4, 15, 0, 0, 0)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_not_return_error_if_later_date_is_undefined()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 0, 0, 0),
				LaterDate = null
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_validate_that_two_dates_with_time_are_valid_when_in_the_proper_order()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 8, 30, 0),
				LaterDate = new DateTime(2008, 4, 15, 9, 30, 0)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_validate_that_two_dates_with_time_are_valid_when_they_are_equal()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15, 9, 30, 0),
				LaterDate = new DateTime(2008, 4, 15, 9, 30, 0)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_validate_that_two_dates_without_time_are_valid_when_in_the_proper_order()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15),
				LaterDate = new DateTime(2008, 4, 16)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}

		[Test]
		public void Should_validate_that_two_dates_without_time_are_valid_when_they_are_equal()
		{
			var rule = new TestDateSequenceRule();
			var operation = new TestOperation
			{
				EarlierDate = new DateTime(2008, 4, 15),
				LaterDate = new DateTime(2008, 4, 15)
			};

			string result = rule.IsValid(operation);
			Assert.Null(result);
		}
	}
}