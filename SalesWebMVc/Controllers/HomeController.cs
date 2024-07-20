using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace SalesWebMVc.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HomeController : ControllerBase
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[SwaggerOperation(Summary = "Obter informações da página inicial")]
		public IActionResult Index()
		{
			var response = "This is a website developed by Dante and Kapo";
			return Ok(new { Body = response }); // Retorna um objeto JSON com o Body
		}
	}
}
