using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models;
using SalesWebMVc.Requests.SalesRequests;
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

		public async Task<IActionResult> GetAll(DateTime? minDate, DateTime? maxDate)
		{
			minDate = minDate ?? new DateTime(2000, 1, 1);
			maxDate = maxDate ?? DateTime.Now;
			var result = await _salesRecordService.GetAllAsync(minDate, maxDate);
			return Ok(result); // Retorna os resultados da busca em formato JSON
		}

		[HttpPost]
		[SwaggerOperation(Summary = "Inserir uma nova venda")]
		[SwaggerResponse(200, "Venda inserida com sucesso")]
		[SwaggerResponse(400, "Erro de validação")]
		[SwaggerResponse(500, "Erro interno do servidor")]
		public async Task<IActionResult> Insert(SaleInsertRequestJson salesRequest)
		{
			SalesRecord newSale = new SalesRecord(DateTime.UtcNow, salesRequest.Amount, salesRequest.Status, salesRequest.SellerId);

			await _salesRecordService.InsertAsync(newSale);
			return Ok();
		}
		[HttpDelete("{id}")]
		[SwaggerOperation(Summary = "Deletar uma venda por Id")]
		[SwaggerResponse(200, "Venda deletada com sucesso")]
		[SwaggerResponse(500, "Erro interno do servidor")]
		[SwaggerResponse(404, "Venda não encontrada")]
		public async Task<IActionResult> Delete(int id)
		{
			await _salesRecordService.RemoveByIdAsnc(id);
			return Ok();
		}
		[HttpGet("{id}")]
		[SwaggerOperation(Summary = "Retornar uma venda por Id")]
		[SwaggerResponse(200, "Venda retornada com sucesso")]
		[SwaggerResponse(500, "Erro interno do servidor")]
		[SwaggerResponse(404, "Venda não encontrada")]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await _salesRecordService.GetByIdAsync(id);
			return Ok(result);
		}

	}
}
