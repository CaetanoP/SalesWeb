using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models;
using SalesWebMVc.Models.ViewModels;
using SalesWebMVc.Services;

namespace SalesWebMVc.Controllers
{
	public class SellersController : Controller
	{

		private readonly SellerService _sellerService;
		private readonly DepartmentService _departmentService;

		public SellersController(SellerService sellerService, DepartmentService departmentService)
		{
			_sellerService = sellerService;
			_departmentService = departmentService;
		}
		public IActionResult Index()
		{
			var list = _sellerService.FindAll();
			//Returning the list to the view in the form of a model
			return View(list);
		}
		public IActionResult Create()
		{
			var departments = _departmentService.FindAll();
			var viewModel = new SellerFormViewModel { Departments = departments };
			//this create returns the view with the form to insert the data
			return View(viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Seller seller ) 
		{ 
			//this create is to insert the data
			_sellerService.Insert(seller);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
				return NotFound();
			}
			return View(obj);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			_sellerService.Remove(id);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
				return NotFound();
			}
			return View(obj);
		}
	}
}
