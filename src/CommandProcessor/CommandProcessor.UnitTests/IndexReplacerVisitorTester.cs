using System;
using System.Linq.Expressions;
using MvcContrib.CommandProcessor.Helpers;
using NUnit.Framework;

namespace MvcContrib.CommandProcessor.UnitTests
{
	[TestFixture]
	public class IndexReplacerVisitorTester
	{
		[Test]
		public void Should_replace_array_index_with_correct_index_value()
		{
			Expression<Func<DrugTestForm, object>> expr = f => f.DrugTestDrugTestResults[int.MaxValue].SubstanceTested;

			var result = new IndexReplacerVisitor(5).Visit(expr);

			Assert.AreEqual("f => f.DrugTestDrugTestResults[5].SubstanceTested", result.ToString());
		}
	}

	public class AbuseForm
	{
		public object AbusePhysical { get; set; }
	}

	public class DrugTestForm
	{
		public object DrugTestId { get; set; }

		public Foo[] DrugTestDrugTestResults { get; set; }

		public object DrugTestWitnessedById { get; set; }
	}

	public class Foo
	{
		public object SubstanceTested { get; set; }
	}
}