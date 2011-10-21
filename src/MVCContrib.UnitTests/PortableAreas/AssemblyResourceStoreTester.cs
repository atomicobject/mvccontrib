using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.PortableAreas;
using MvcContrib.UI.InputBuilder.ViewEngine;
using NUnit.Framework;
using VB = MVCContrib.UnitTests.VB;

namespace MvcContrib.UnitTests.PortableAreas
{
    [TestFixture]
    public class AssemblyResourceStoreTester
    {
        /// <summary>
        /// Ensure area registration only happens one time.
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
        	PortableAreaRegistration.RegisterEmbeddedViewEngine = () => { };
        	PortableAreaRegistration.CheckAreasWebConfigExists = () => { };
            RegisterTestAreas();
        }

        [Test]
        public void Assembly_resource_store_should_find_embedded_images()
        {
            VerifiyAssemblyResourceStoreIsPathResourceStream(StubPortableAreaRegistrationForResourceStoreTest.PortableAreaName);
        }

        [Test]
        public void Assembly_resource_store_should_find_embedded_images_VB_at_root()
        {
            VerifiyAssemblyResourceStoreIsPathResourceStream(VB.PortableAreaRegistrationAtRoot.PortableAreaName);
        }

        [Test]
        public void Assembly_resource_store_should_find_embedded_images_VB_under_subnamespace()
        {
            VerifiyAssemblyResourceStoreIsPathResourceStream(VB.PortableAreas.PortableAreaRegistrationUnderSubnamespace.PortableAreaName);
        }

        private void VerifiyAssemblyResourceStoreIsPathResourceStream(string areaName) {
            AssemblyResourceStore store;

            InitializeEmbeddedResourceController(areaName);
            store = AssemblyResourceManager.GetResourceStoreForArea(areaName);
            store.ShouldNotBeNull();

            store.IsPathResourceStream("~/Images/arrow.gif").ShouldBeTrue();
            store.IsPathResourceStream("~/Images/Missing_Image.gif").ShouldBeFalse();
        }

        private static EmbeddedResourceController InitializeEmbeddedResourceController(string areaName) {
            var controller = new EmbeddedResourceController();
            var routeData = new RouteData();
            routeData.DataTokens.Add("area", areaName);
            controller.ControllerContext = new ControllerContext(MvcMockHelpers.DynamicHttpContextBase(), routeData, controller);
            return controller;
        }

        private void RegisterTestAreas()
        {
            RegisterTestArea();
            RegisterTestAreaForVbAtRoot();
            RegisterTestAreaForVbUnderSubNamespace();
        }

        private void RegisterTestArea()
        {
            RegisterTestArea(new StubPortableAreaRegistration(), "FooArea");
        }

        private void RegisterTestArea(PortableAreaRegistration areaRegistration, string areaName)
        {
            var registrationContext = new AreaRegistrationContext(areaName, new RouteCollection());
            TestingAreaRegistration.Register(areaRegistration, registrationContext);
        }

        private void RegisterTestAreaForVbAtRoot()
        {
            RegisterTestArea(new VB.PortableAreaRegistrationAtRoot(),
                             VB.PortableAreaRegistrationAtRoot.PortableAreaName);
        }

        private void RegisterTestAreaForVbUnderSubNamespace()
        {
            RegisterTestArea(new VB.PortableAreas.PortableAreaRegistrationUnderSubnamespace(),
                             VB.PortableAreas.PortableAreaRegistrationUnderSubnamespace.PortableAreaName);
        }
    }

    class StubPortableAreaRegistrationForResourceStoreTest : PortableAreaRegistration
    {
        public const string PortableAreaName = "FooArea";

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            context.MapRoute("ResourceRoute", "fooarea/resource/{resourceName}", 
                new { controller = "Resource", action = "Index" });

            
            this.RegisterAreaEmbeddedResources();
        }

        public override string AreaName
        {
            get
            {
                return PortableAreaName;
            }
        }
    }
}
