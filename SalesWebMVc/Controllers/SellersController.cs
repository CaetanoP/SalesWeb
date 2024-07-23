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
		[SwaggerOperation(Summary = "Obter todos os vendedores")]
		[SwaggerResponse(200, "Vendedores encontrados", typeof(IEnumerable<Seller>))]
		public async Task<ActionResult<IEnumerable<Seller>>> GetAll()
		{
			var list = await _sellerService.FindAllAsync();
			return Ok(list);
		}

		[HttpGet("{id}")] // Detalhes de um vendedor por ID
		[SwaggerOperation(Summary = "Obter detalhes de um vendedor")]
		[SwaggerResponse(200, "Vendedor encontrado", typeof(SellerResponseDetailJson))]
		[SwaggerResponse(404, "Vendedor não encontrado")]
		public async Task<ActionResult<SellerResponseDetailJson>> Details(int id)
		{
			var obj = await _sellerService.FindByIdAsync(id);
			var response = new SellerResponseDetailJson(obj);
			return Ok(response);
		}

		[HttpPost] // Criar um novo vendedor
		[SwaggerOperation(Summary = "Criar um novo vendedor")]
		[SwaggerResponse(201, "Vendedor criado", typeof(SellerResponseCreateJson))]
		public async Task<ActionResult<Seller>> Create(SellerRequestCreateJson sellerRequest)
		{
			Seller newSeller = new Seller( sellerRequest.Name, sellerRequest.Email, sellerRequest.BirthDate, sellerRequest.BaseSalary, sellerRequest.DepartmentId);

			await _sellerService.InsertAsync(newSeller);
			SellerResponseCreateJson responseCreateJson = new SellerResponseCreateJson(newSeller.Id, newSeller.Name, newSeller.Email, newSeller.BaseSalary, newSeller.BirthDate, newSeller.DepartmentId);
			return CreatedAtAction(nameof(Details), new { id = responseCreateJson.Id }, responseCreateJson);
		}

		[HttpPut("{id}")] // Editar um vendedor existente
		[SwaggerOperation(Summary = "Atualizar um vendedor existente")]
		[SwaggerResponse(204, "Vendedor atualizado")]
		public async Task<IActionResult> UpdateAsync(int id, SellerRequestUpdateJson sellerRequest)
		{
			//Modificar depois para instanciar com o contrutor
			Seller newSeller = new Seller(sellerRequest.Name, sellerRequest.Email, sellerRequest.BirthDate, sellerRequest.BaseSalary, sellerRequest.DepartmentId);
			await _sellerService.UpdateAsync(newSeller);
			return NoContent(); // 204 no Content

		}
		[HttpDelete("{id}")] // Excluir um vendedor
		[SwaggerOperation(Summary = "Excluir um vendedor")]
		public async Task<IActionResult> Delete(int id)
		{
			await _sellerService.RemoveAsync(id);
			return NoContent(); // Retorna 204 No Content
		}
	}
}
