using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Binders;
using MvcContrib.Samples.UI.Models.DerivedTypeModelBinder;

namespace MvcContrib.Samples.UI
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			DerivedTypeModelBinderCache.RegisterDerivedTypes(typeof (IContent), new[] {typeof (AddressInfo), typeof (User)});

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
		}
	}
}