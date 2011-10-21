using System;
using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace $safeprojectname$.Areas
{
	public class DefaultController : Controller
	{
		public ActionResult Index()
		{
			return View((object)"Hello World");
		}
	}
}