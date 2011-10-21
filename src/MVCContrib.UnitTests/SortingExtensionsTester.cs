using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using MvcContrib.Sorting;
namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class SortingExtensionsTester
	{
		private List<Person> _data;

		[SetUp]
		public void Setup()
		{
			_data = new List<Person>
			{
				new Person() { Name="B", Id=2 },
				new Person() { Name = "C", Id=3},
				new Person() { Name = "A", Id=1}
			};
		}

		[Test]
		public void Sorts_by_name_ascending()
		{
			var data = _data.OrderBy("Name", SortDirection.Ascending);
			data.ElementAt(0).Name.ShouldEqual("A");
			data.ElementAt(1).Name.ShouldEqual("B");
			data.ElementAt(2).Name.ShouldEqual("C");

		}

		[Test]
		public void Sorts_by_name_descending()
		{
			var data = _data.OrderBy("Name", SortDirection.Descending);
			data.ElementAt(0).Name.ShouldEqual("C");
			data.ElementAt(1).Name.ShouldEqual("B");
			data.ElementAt(2).Name.ShouldEqual("A");
		}

		[Test]
		public void Sorts_by_id_ascending()
		{
			var data = _data.OrderBy("Id", SortDirection.Ascending);
			data.ElementAt(0).Id.ShouldEqual(1);
			data.ElementAt(1).Id.ShouldEqual(2);
			data.ElementAt(2).Id.ShouldEqual(3);

		}

		[Test]
		public void Sorts_by_id_descending()
		{
			var data = _data.OrderBy("Id", SortDirection.Descending);
			data.ElementAt(0).Id.ShouldEqual(3);
			data.ElementAt(1).Id.ShouldEqual(2);
			data.ElementAt(2).Id.ShouldEqual(1);
		}

		[Test]
		public void Does_not_perform_sort_if_column_null()
		{
			var data = _data.OrderBy(null, SortDirection.Ascending);
			data.ElementAt(0).Name.ShouldEqual("B");
			data.ElementAt(1).Name.ShouldEqual("C");
			data.ElementAt(2).Name.ShouldEqual("A");
		}

		[Test]
		public void Throws_when_property_name_invalid()
		{
			Assert.Throws<InvalidOperationException>(() => _data.OrderBy("foo", SortDirection.Ascending));
		}


		private class Person
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}