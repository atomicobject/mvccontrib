using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using MvcContrib.ActionResults;
using NUnit.Framework;

using Rhino.Mocks;
using System.Xml.Serialization;

namespace MvcContrib.UnitTests.ActionResults
{
	[TestFixture]
	public class XmlResultTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<ControllerBase>());
			_mocks.ReplayAll();
		}

		[Test]
		public void ObjectToSerialize_should_return_the_object_to_serialize()
		{
			var result = new XmlResult(new Person {Id = 1, Name = "Bob"});
			Assert.That(result.ObjectToSerialize, Is.InstanceOf<Person>());
			Assert.That(((Person)result.ObjectToSerialize).Name, Is.EqualTo("Bob"));
		}

		[Test]
		public void Should_set_content_type()
		{
			var result = new XmlResult(new[] {2, 3, 4});
			result.ExecuteResult(_controllerContext);
			Assert.AreEqual("text/xml", _controllerContext.HttpContext.Response.ContentType);
		}

		[Test]
		public void Should_serialise_xml()
		{
			var result = new XmlResult(new Person {Id = 5, Name = "Jeremy"});
			result.ExecuteResult(_controllerContext);

			var doc = new XmlDocument();
			doc.LoadXml(_controllerContext.HttpContext.Response.Output.ToString());
			Assert.That(doc.SelectSingleNode("/Person/Name").InnerText, Is.EqualTo("Jeremy"));
			Assert.That(doc.SelectSingleNode("/Person/Id").InnerText, Is.EqualTo("5"));
		}

		[Test]
		public void XmlOverrides_can_change_root_node()
		{
			var people = new System.Collections.Generic.List<Person>() {
											new Person(){ Id = 5, Name = "Jeremy" },
											new Person(){ Id = 1, Name = "Bob" }
			};

			var attributes = new XmlAttributes();
			attributes.XmlRoot = new XmlRootAttribute("People");

			var xmlAttribueOverrides = new XmlAttributeOverrides();
			xmlAttribueOverrides.Add(people.GetType(), attributes);

			var result = new XmlResult(people, xmlAttribueOverrides);
			result.ExecuteResult(_controllerContext);

			var doc = new XmlDocument();
			doc.LoadXml(_controllerContext.HttpContext.Response.Output.ToString());
			Assert.That(doc.DocumentElement.Name, Is.EqualTo("People"));
		}

		public class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}
