using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.PortableAreas;
using NUnit.Framework;
using VB = MVCContrib.UnitTests.VB;

namespace MvcContrib.UnitTests.PortableAreas
{
    [TestFixture]
    public class EmbeddedResourceControllerTester
    {
        /// <summary>
        /// Ensure area registration only happens one time.
        /// </summary>
        [TestFixtureSetUp]
        public void Embedded_resource_controller_setup()
        {
        	PortableAreaRegistration.RegisterEmbeddedViewEngine = () => { };
        	PortableAreaRegistration.CheckAreasWebConfigExists = () => { };
            RegisterTestAreas();
        }

        [Test]
        public void Embedded_resource_controller_should_return_embedded_image()
        {
            VerifyEmbeddedResourceControllerReturnEmbeddedImage(InitializeEmbeddedResourceController());
        }

        [Test]
        public void Embedded_resource_controller_should_return_embedded_image_VB_at_root()
        {
            VerifyEmbeddedResourceControllerReturnEmbeddedImage(InitializeEmbeddedResourceControllerForVbAreaAtRoot());
        }

        [Test]
        public void Embedded_resource_controller_should_return_embedded_image_VB_under_subnamespace()
        {
            VerifyEmbeddedResourceControllerReturnEmbeddedImage(InitializeEmbeddedResourceControllerForVbAreaUnderSubNamespace());
        }

        [Test]
        public void Embedded_resource_controller_should_return_404_for_nonexistant_resource()
        {
            VerifyEmbeddedResourceControllerReturn404ForNonexistantResource(InitializeEmbeddedResourceController());
        }

        [Test]
        public void Embedded_resource_controller_should_return_404_for_nonexistant_resource_VB_at_root()
        {
            VerifyEmbeddedResourceControllerReturn404ForNonexistantResource(InitializeEmbeddedResourceControllerForVbAreaAtRoot());
        }

        [Test]
        public void Embedded_resource_controller_should_return_404_for_nonexistant_resource_VB_under_subnamespace()
        {
            VerifyEmbeddedResourceControllerReturn404ForNonexistantResource(InitializeEmbeddedResourceControllerForVbAreaUnderSubNamespace());
        }

        [Test]
        public void Embedded_resource_controller_should_return_embedded_image_for_custom_path()
        {
            VerifyEmbeddedResourceControllerReturnEmbeddedImageForCustomPath(InitializeEmbeddedResourceController());
        }

        [Test]
        public void Embedded_resource_controller_should_return_embedded_image_for_custom_path_VB_at_root()
        {
            VerifyEmbeddedResourceControllerReturnEmbeddedImageForCustomPath(InitializeEmbeddedResourceControllerForVbAreaAtRoot());
        }

        [Test]
        public void Embedded_resource_controller_should_return_embedded_image_for_custom_path_VB_under_subnamespace()
        {
            VerifyEmbeddedResourceControllerReturnEmbeddedImageForCustomPath(InitializeEmbeddedResourceControllerForVbAreaUnderSubNamespace());
        }

        [Test]
        public void Embedded_resource_controller_should_return_404_for_nonexistant_custom_path()
        {
            VerifyEmbeddedResourceControllerReturn404ForNonexistantCustomPath(InitializeEmbeddedResourceController());
        }

        [Test]
        public void Embedded_resource_controller_should_return_404_for_nonexistant_custom_path_VB_at_root()
        {
            VerifyEmbeddedResourceControllerReturn404ForNonexistantCustomPath(InitializeEmbeddedResourceControllerForVbAreaAtRoot());
        }

        [Test]
        public void Embedded_resource_controller_should_return_404_for_nonexistant_custom_path_VB_under_subnamespace()
        {
            VerifyEmbeddedResourceControllerReturn404ForNonexistantCustomPath(InitializeEmbeddedResourceControllerForVbAreaUnderSubNamespace());
        }

        private static EmbeddedResourceController InitializeEmbeddedResourceController()
        {
            return InitializeEmbeddedResourceController("FooArea");
        }

        private static EmbeddedResourceController InitializeEmbeddedResourceController(string areaName) {
            var controller = new EmbeddedResourceController();
            var routeData = new RouteData();
            routeData.DataTokens.Add("area", areaName);
            controller.ControllerContext = new ControllerContext(MvcMockHelpers.DynamicHttpContextBase(), routeData, controller);
            return controller;
        }

        private static EmbeddedResourceController InitializeEmbeddedResourceControllerForVbAreaAtRoot()
        {
            return InitializeEmbeddedResourceController(VB.PortableAreaRegistrationAtRoot.PortableAreaName);
        }

        private static EmbeddedResourceController InitializeEmbeddedResourceControllerForVbAreaUnderSubNamespace()
        {
            return InitializeEmbeddedResourceController(VB.PortableAreas.PortableAreaRegistrationUnderSubnamespace.PortableAreaName);
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

        private void VerifyEmbeddedResourceControllerReturnEmbeddedImage(EmbeddedResourceController resourceController)
        {
            // act
            var result = resourceController.Index("images.arrow.gif", null) as FileStreamResult;

            // assert
            result.FileStream.ShouldNotBeNull();
            result.ContentType.ShouldEqual("image/gif");
        }

        private void VerifyEmbeddedResourceControllerReturn404ForNonexistantResource(EmbeddedResourceController resourceController)
        {
            // act
            var result = resourceController.Index("foobar.gif", null);

            // assert
            result.ShouldBeNull();
            resourceController.Response.StatusCode.ShouldEqual(404);
        }

        private void VerifyEmbeddedResourceControllerReturnEmbeddedImageForCustomPath(EmbeddedResourceController resourceController)
        {
            // act
            var result = resourceController.Index("arrow.gif", "images") as FileStreamResult;

            // assert
            result.FileStream.ShouldNotBeNull();
            result.ContentType.ShouldEqual("image/gif");
        }

        private void VerifyEmbeddedResourceControllerReturn404ForNonexistantCustomPath(EmbeddedResourceController resourceController)
        {
            // act
            var result = resourceController.Index("foobar.gif", "ximages");

            // assert
            result.ShouldBeNull();
            resourceController.Response.StatusCode.ShouldEqual(404);
        }
    }

    class StubPortableAreaRegistration : PortableAreaRegistration
    {
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
                return "FooArea";
            }
        }
    }
}
