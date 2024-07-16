using Microsoft.AspNetCore.Mvc;

namespace SalesWebMVc.Controllers
{
    public class SellersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
