using System;
using MvcContrib.UI.InputBuilder.ViewEngine;
using NUnit.Framework;
using MvcContrib.PortableAreas;

namespace MvcContrib.UnitTests.UI.InputBuilder
{
	[TestFixture]
	public class AssemblyResourceProviderTester
	{
		[Test]
		public void GetCacheKey_should_return_null()
		{
			//arrange
			var provider = new AssemblyResourceProvider();
            
			//act
			var result = provider.GetCacheKey("");

			//assert
			Assert.IsNull(result);
		}

        [Test]
        public void GetFile_should_return_virtual_file()
        {
            //arrange
            var provider = new AssemblyResourceProvider();

            //act
            var result = provider.GetFile("~/Views/InputBuilders/String.aspx");

            //assert
            Assert.IsNotNull(result);
        }

		[Test]
		public void App_resource_path_should_find_input_builders()
		{
            //arrange/act
            var result = AssemblyResourceManager.IsEmbeddedViewResourcePath("~/Views/InputBuilders/String.aspx");

            //assert
            Assert.IsTrue(result);
		}

		[Test]
		public void Get_cache_dep_should_return_null_for_builders()
		{
			//arrange
			var provider = new AssemblyResourceProvider();
            
			//act
			var result = provider.GetCacheDependency("~/Views/InputBuilders/foo.aspx", new string[0], DateTime.Now);

			//assert
			Assert.IsNull(result);

		}

		[Test]
		public void File_exists()
		{
			//arrange
			var provider = new AssemblyResourceProvider();

			//act
			var result = provider.FileExists("~/foo");

			//assert
			Assert.IsFalse(result);

		}
        
	}
}