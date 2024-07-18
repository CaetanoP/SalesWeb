using Microsoft.AspNetCore.Mvc;
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
	}
}
