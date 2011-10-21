using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests
{
	public static class ExecutionResultAssertionExtensions
	{
		public static string WrapEachWith<T>(this IEnumerable<T> values, string before, string after, string separator)
		{
			return string.Join(separator, values.Select(x => string.Format("{0}{1}{2}", before, x, after)).ToArray());
		}


		public static void ShouldHaveMessage<TMessage>(this ExecutionResult result,
		                                               Expression<Func<TMessage, object>> expression)
		{
			if (result.Messages.Where(x => x.IncorrectAttribute == expression).Count() == 0)
			{
				string failureMessage = result.Messages.Select(x => x.IncorrectAttribute + ":" + x.MessageText).WrapEachWith("", "",
				                                                                                                             "\n");
				Assert.Fail(string.Format("No message for {0}.  Other messages include:\n{1}", expression, failureMessage));
				return;
			}
		}

		public static void ShouldNotBeSuccessful(this ExecutionResult result)
		{
			Assert.That(result.Successful, Is.False);
		}

		public static void ShouldHaveMessage(this ExecutionResult result, string key)
		{
		}

		public static void ShouldBeSuccessful(this ExecutionResult result)
		{
			if (!result.Successful)
			{
				string failureMessage = result.Messages.Select(x => x.IncorrectAttribute + ":" + x.MessageText).WrapEachWith("", "",
				                                                                                                             "\n");
				Assert.Fail(string.Format("Not successful. Messages include:\n{0}", failureMessage));
			}
		}
	}
}