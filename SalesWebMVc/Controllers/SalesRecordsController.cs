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

		[HttpGet("simpleSearch")]
		[SwaggerOperation(Summary = "Realiza uma busca simples por registros de venda")]
		public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
		{
			minDate = minDate ?? new DateTime(DateTime.Now.Year, 1, 1);
			maxDate = maxDate ?? DateTime.Now;

			var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
			return Ok(result); // Retorna os resultados da busca em formato JSON
		}

		[HttpGet("groupingSearch")]
		[SwaggerOperation(Summary = "Realiza uma busca agrupada por registros de venda")]
		public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
		{
			minDate = minDate ?? new DateTime(DateTime.Now.Year, 1, 1);
			maxDate = maxDate ?? DateTime.Now;

			var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
			return Ok(result); // Retorna os resultados da busca em formato JSON
		}
	}
}
