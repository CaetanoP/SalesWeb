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
		[SwaggerOperation(
		   Summary = "Obter todas as vendas",
		   Description = "Retorna uma lista de todas as vendas em um determinado período. Utilize os parâmetros opcionais 'minDate' e 'maxDate' para filtrar por período. Deixe em branco para retornar todas as vendas"
	   )]
		[SwaggerResponse(200, "Vendas encontradas", typeof(IEnumerable<SalesRecord>))]
		[SwaggerResponse(404, "Nenhuma venda encontrada")]
		public async Task<ActionResult<IEnumerable<SalesRecord>>> GetAll([FromQuery] DateTime? minDate,
			[FromQuery] DateTime? maxDate)
		{
			minDate = minDate ?? new DateTime(2000, 1, 1);
			maxDate = maxDate ?? DateTime.Now;
			var result = await _salesRecordService.GetAllAsync(minDate, maxDate);
			return Ok(result); // Retorna os resultados da busca em formato JSON
		}




		[HttpPost]
		[SwaggerOperation(
			Summary = "Criar um novo registro de venda",
			Description = "Cria um novo registro de venda com os dados fornecidos no corpo da requisição."
		)]
		[SwaggerResponse(201, "Venda criada com sucesso", typeof(SalesRecord))]
		[SwaggerResponse(400, "Dados da venda inválidos")]
		public async Task<ActionResult<SalesRecord>> Create(SaleInsertRequestJson salesRequest)
		{
			SalesRecord newSale = new SalesRecord(DateTime.UtcNow, salesRequest.Amount, salesRequest.Status, salesRequest.SellerId);

			await _salesRecordService.CreateAsync(newSale);
			return CreatedAtAction(nameof(GetById), new { id = newSale.Id }, newSale);
		}



		[HttpDelete("{id}")]
		[SwaggerOperation(
		   Summary = "Excluir uma venda por ID",
		   Description = "Exclui o registro de venda com o ID especificado."
	   )]
		[SwaggerResponse(204, "Venda excluída com sucesso")]
		[SwaggerResponse(404, "Venda não encontrada")]
		public async Task<IActionResult> Delete(int id)
		{
			await _salesRecordService.RemoveByIdAsnc(id);
			return Ok();
		}



		[HttpGet("{id}")]
		[SwaggerOperation(
			Summary = "Obter detalhes de uma venda por ID",
			Description = "Retorna os detalhes da venda com o ID especificado."
		)]
		[SwaggerResponse(200, "Venda encontrada", typeof(SalesRecord))]
		[SwaggerResponse(404, "Venda não encontrada")]
		public async Task<ActionResult<SalesRecord>> GetById(int id)
		{
			var result = await _salesRecordService.GetByIdAsync(id);
			return Ok(result);
		}

	}
}
