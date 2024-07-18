using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models;
using SalesWebMVc.Services;

namespace SalesWebMVc.Controllers
{
	public class SellersController : Controller
	{

		private readonly SellerService _sellerService;

		public SellersController(SellerService sellerService)
		{
			_sellerService = sellerService;
		}
		public IActionResult Index()
		{
			var list = _sellerService.FindAll();
			//Returning the list to the view in the form of a model
			return View(list);
		}
		public IActionResult Create()
		{
			//this create is to return the view
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Seller seller ) 
		{ 
			//this create is to insert the data
			_sellerService.Insert(seller);
			return RedirectToAction(nameof(Index));
		}
	}
}
