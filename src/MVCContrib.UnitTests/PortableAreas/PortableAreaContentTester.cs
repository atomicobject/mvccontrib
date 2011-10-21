using NUnit.Framework;
using MvcContrib.PortableAreas;

namespace MvcContrib.UnitTests.PortableAreas
{
	[TestFixture]
	public class PortableAreaContentTester
	{
		[SetUp]
		public void SetUp()
		{
			PortableAreaContent.Maps = null;
		}

		[Test]
		public void PortableAreaContent_Map_should_return_map()
		{
			var map = PortableAreaContent.Map<fooMap>();
			map.ShouldNotBeNull();
		}

		[Test]
		public void PortableAreaContent_Map_should_retain_values()
		{
			PortableAreaContent.Map<fooMap>().Master("SomeMaster");
			var map = PortableAreaContent.Map<fooMap>();
			map.MasterPageLocation.ShouldBeTheSameAs("SomeMaster");
		}

		[Test]
		public void PortableAreaContent_Map_should_retain_custom_values()
		{
			PortableAreaContent.Map<barMap>().OtherContent("SomeOtherContent");
			var map = PortableAreaContent.Map<barMap>();
			map.OtherContentValue.ShouldBeTheSameAs("SomeOtherContent");
		}

		[Test]
		public void PortableAreaContent_MapAll_should_provide_default_values()
		{
			PortableAreaContent.MapAll().Master("SomeMaster");
			var map = PortableAreaContent.Map<fooMap>();
			map.MasterPageLocation.ShouldBeTheSameAs("SomeMaster");
		}

		[Test]
		public void PortableAreaContent_MapAll_defaults_can_be_overridden()
		{
			PortableAreaContent.MapAll().Master("SomeMaster");
			PortableAreaContent.Map<fooMap>().Master("SomeOtherMaster");
			var map = PortableAreaContent.Map<fooMap>();
			map.MasterPageLocation.ShouldBeTheSameAs("SomeOtherMaster");
		}
	}

	class fooMap : PortableAreaMap { }
	class barMap : PortableAreaMap
	{
		public barMap()
		{
			DefaultMasterPageLocation = "Kalamazoo";
		}
		public barMap OtherContent(string name)
		{
			OtherContentValue = name;
			Add("OtherContent", name);
			return this;
		}
		public string OtherContentValue { get; set; }
	}
}
