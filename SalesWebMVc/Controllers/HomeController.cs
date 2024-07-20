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
			var email = "caetanof2004@gmail.com";
			return Ok(new { Email = email }); // Retorna um objeto JSON com o email
		}
	}
}
