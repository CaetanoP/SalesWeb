using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol.Plugins;
using SalesWebMVc.Filter;
using SalesWebMVc.Models;
using SalesWebMVc.Requests.SellerRequests;
using SalesWebMVc.Responses.SellerResponses;
using SalesWebMVc.Services;
using SalesWebMVc.Services.Exceptions;
using Swashbuckle.AspNetCore.Annotations;


namespace SalesWebMVc.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SellersController : ControllerBase
	{
		private readonly SellerService _sellerService;
		private readonly DepartmentService _departmentService;

		public SellersController(SellerService sellerService, DepartmentService departmentService)
		{
			_sellerService = sellerService;
			_departmentService = departmentService;
		}


		[HttpGet] // Lista todos os vendedores
		[SwaggerOperation(
			Summary = "Obter todos os vendedores",
			Description = "Retorna uma lista de todos os vendedores cadastrados. Use o parâmetro 'includeSales' para incluir ou não os detalhes das vendas de cada vendedor."
		)]
		[SwaggerResponse(200, "Vendedores encontrados", typeof(IEnumerable<Seller>))]
		public async Task<ActionResult<IEnumerable<Seller>>> GetAll([FromQuery] bool includeSales = false)
		{
			if (includeSales)
			{
				var list = await _sellerService.FindAllAsyncWithDetails();
				return Ok(list);

			}
			else
			{
				var list = await _sellerService.FindAllAsyncWithoutDetails();
				return Ok(list);
			}
		}



		[HttpGet("{id}")]
		[SwaggerOperation(
			Summary = "Obter detalhes de um vendedor",
			Description = "Retorna os detalhes do vendedor com o ID especificado."
		)]
		[SwaggerResponse(200, "Vendedor encontrado", typeof(SellerResponseDetailJson))]
		[SwaggerResponse(404, "Vendedor não encontrado")]
		public async Task<ActionResult<SellerResponseDetailJson>> Details(int id)
		{
			var obj = await _sellerService.FindByIdAsync(id);
			var response = new SellerResponseDetailJson(obj);
			return Ok(response);
		}



		[HttpPost]
		[SwaggerOperation(
			Summary = "Criar um novo vendedor",
			Description = "Cria um novo vendedor com os dados fornecidos no corpo da requisição."
		)]
		[SwaggerResponse(201, "Vendedor criado", typeof(SellerResponseCreateJson))]
		[SwaggerResponse(400, "Dados do vendedor inválidos")]
		public async Task<ActionResult<Seller>> Create(SellerRequestCreateJson sellerRequest)
		{
			Seller newSeller = new Seller(sellerRequest.Name, sellerRequest.Email, sellerRequest.BirthDate, sellerRequest.BaseSalary, sellerRequest.DepartmentId);

			await _sellerService.InsertAsync(newSeller);
			SellerResponseCreateJson responseCreateJson = new SellerResponseCreateJson(newSeller.Id, newSeller.Name, newSeller.Email, newSeller.BaseSalary, newSeller.BirthDate, newSeller.DepartmentId);
			return CreatedAtAction(nameof(Details), new { id = responseCreateJson.Id }, responseCreateJson);
		}



		[HttpPut("{id}")]
		[SwaggerOperation(
		   Summary = "Atualizar um vendedor existente",
		   Description = "Atualiza os dados do vendedor com o ID especificado. Os dados atualizados devem ser fornecidos no corpo da requisição."
	   )]
		[SwaggerResponse(204, "Vendedor atualizado")]
		[SwaggerResponse(400, "Dados do vendedor inválidos")]
		[SwaggerResponse(404, "Vendedor não encontrado")]
		public async Task<IActionResult> UpdateAsync(int id, SellerRequestUpdateJson sellerRequest)
		{
			//Modificar depois para instanciar com o contrutor
			Seller newSeller = new Seller(id, sellerRequest.Name, sellerRequest.Email, sellerRequest.BirthDate, sellerRequest.BaseSalary, sellerRequest.DepartmentId);
			await _sellerService.UpdateAsync(newSeller);
			return NoContent(); // 204 no Content

		}


		[HttpDelete("{id}")]
		[SwaggerOperation(
		   Summary = "Excluir um vendedor",
		   Description = "Exclui o vendedor com o ID especificado."
	   )]
		[SwaggerResponse(204, "Vendedor excluído")]
		[SwaggerResponse(404, "Vendedor não encontrado")]
		public async Task<IActionResult> Delete(int id)
		{
			await _sellerService.RemoveAsync(id);
			return NoContent(); // Retorna 204 No Content
		}
	}
}
