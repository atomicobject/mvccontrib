using System;
using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace MvcContrib.UnitTests.PortableAreas
{
    internal class TestingAreaRegistration
    {
        /// <summary>
        /// Register the received area with the associated context while ignoring potential exception
        /// about already registered area.
        /// </summary>
        public static void Register(PortableAreaRegistration areaRegistration, AreaRegistrationContext registrationContext)
        {
            try
            {
                areaRegistration.RegisterArea(registrationContext);
            }
            catch (ArgumentException e)
            {
                if (!e.Message.Contains("An item with the same key has already been added."))
                    throw;
            }

        }
    }
}
