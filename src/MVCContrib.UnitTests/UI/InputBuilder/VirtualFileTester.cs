using MvcContrib.UI.InputBuilder.ViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class VirtualFileTester
	{

		[Test]
		public void The_file_should_locate_a_embedded_resource()
		{
			//arrange
            var resourceStore = new AssemblyResourceStore(typeof(AssemblyResourceProvider), "", "MvcContrib.UI.InputBuilder");
            var file = new AssemblyResourceVirtualFile("~/Views/InputBuilders/String.aspx", resourceStore);

			//act
			var result = file.Open();

			//assert
			Assert.IsNotNull(result);
		}

		[Test]
		public void The_file_should_not_locate_an_invalid_path()
		{
			//arrange
            var resourceStore = new AssemblyResourceStore(typeof(AssemblyResourceProvider), "", "MvcContrib.UI.InputBuilder");
			
            var file = new AssemblyResourceVirtualFile("~/foo", resourceStore);

			//act
			var result = file.Open();

			//assert
			Assert.IsNull(result);
		}
	}
}