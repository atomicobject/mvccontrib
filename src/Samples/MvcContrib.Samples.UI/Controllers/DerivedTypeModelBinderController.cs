using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Samples.UI.Models.DerivedTypeModelBinder;

namespace MvcContrib.Samples.UI.Controllers
{
    public class DerivedTypeModelBinderController : Controller
    {
        //
        // GET: /DerivedTypeModelBinder/

        public ActionResult Index()
        {
        	var model = new CustomerInfo
        	            	{
        	            		Contents = new List<IContent>(new IContent[] {new User(), new AddressInfo()})
        	            	};

        	return View(model);
        }

		[HttpPost]
		public ActionResult Index(CustomerInfo customerInfo)
		{
			TempData["customerInfo"] = customerInfo;

			return RedirectToAction("ResultSummary");
		}

		public ActionResult ResultSummary()
		{
			var item = TempData["customerInfo"] as CustomerInfo;
			return View(item);
		}
    }
}
