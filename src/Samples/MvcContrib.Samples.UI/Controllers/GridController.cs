using System.Linq;
using System.Web.Mvc;
using MvcContrib.Samples.UI.Models;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
namespace MvcContrib.Samples.UI.Controllers
{
	public class GridController : Controller
	{
		private readonly PeopleFactory _peopleFactory = new PeopleFactory();

		public ActionResult Index()
		{
			return View(_peopleFactory.CreatePeople());
		}

		public ActionResult Show(int id)
		{
			var person = _peopleFactory.CreatePeople().Where(x => x.Id == id).SingleOrDefault();
			return View(person);
		}

		public ActionResult Paged(int? page)
		{
			var pagedPeople = _peopleFactory.CreatePeople().AsPagination(page.GetValueOrDefault(1), 10);
			return View(pagedPeople);
		}

		public ActionResult WithSections()
		{
			return View(_peopleFactory.CreatePeople());
		}

		public ActionResult UsingGridModel()
		{
			return View(_peopleFactory.CreatePeople());
		}

		public ActionResult AutoColumns() {
			return View(_peopleFactory.CreatePeople());
		}

		public ActionResult Sorting(GridSortOptions sort) 
		{
			ViewData["sort"] = sort;
			var people = _peopleFactory.CreatePeople();

			if(!string.IsNullOrEmpty(sort.Column)) {
				people = people.OrderBy(sort.Column, sort.Direction);
			}

			return View(people);
		}

		public ActionResult SortingAndPaging(int? page, GridSortOptions sort) 
		{
			ViewData["sort"] = sort;
			var people = _peopleFactory.CreatePeople();

			if (!string.IsNullOrEmpty(sort.Column)) {
				people = people.OrderBy(sort.Column, sort.Direction);
			}

			people = people.AsPagination(page ?? 1, 10);

			return View(people);
		}
	}
}