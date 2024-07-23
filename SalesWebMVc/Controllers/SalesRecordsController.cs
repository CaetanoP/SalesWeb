using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SalesWebMVc.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SalesRecordsController : ControllerBase
	{
		private readonly SalesRecordService _salesRecordService;

		public SalesRecordsController(SalesRecordService salesRecordService)
		{
			_salesRecordService = salesRecordService;
		}

		[HttpGet]
		[SwaggerOperation(Summary = "Retornar todas as vendas")]
		[SwaggerResponse(200, "Vendas retornadas com sucesso")]
		[SwaggerResponse(500, "Erro interno do servidor")]
		[SwaggerResponse(404, "Nenhuma venda encontrada")]
		
		public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
		{
			minDate = minDate ?? new DateTime(2000, 1, 1);
			maxDate = maxDate ?? DateTime.Now;
			var result = await _salesRecordService.GetAllAsync(minDate, maxDate);
			return Ok(result); // Retorna os resultados da busca em formato JSON
		}

	}
}
