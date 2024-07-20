using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models;
using SalesWebMVc.Services;
using SalesWebMVc.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Services.Exceptions;
using System.Diagnostics;
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
		public async Task<ActionResult<IEnumerable<Seller>>> Index()
		{
			var list = await _sellerService.FindAllAsync();
			return Ok(list);
		}

		[HttpGet("{id}")] // Detalhes de um vendedor por ID
		[SwaggerOperation(Summary = "Obter detalhes de um vendedor")]
		public async Task<ActionResult<Seller>> Details(int id)
		{
			var obj = await _sellerService.FindByIdAsync(id);
			if (obj == null)
			{
				return NotFound(); // Retorna 404 Not Found
			}
			return Ok(obj);
		}

		[HttpPost] // Criar um novo vendedor
		[SwaggerOperation(Summary = "Criar um novo vendedor")]
		public async Task<ActionResult<Seller>> Create(Seller seller)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Retorna 400 Bad Request
			}
			await _sellerService.InsertAsync(seller);
			return CreatedAtAction(nameof(Details), new { id = seller.Id }, seller);
		}

		[HttpPut("{id}")] // Editar um vendedor existente
		[SwaggerOperation(Summary = "Atualizar um vendedor existente")]
		public async Task<IActionResult> Edit(int id, Seller seller)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Retorna 400 Bad Request
			}
			if (id != seller.Id)
			{
				return BadRequest(); // Retorna 400 Bad Request
			}
			try
			{
				var existingSeller = await _sellerService.FindByIdAsync(id);
				if (existingSeller == null)
				{
					return NotFound(); // Retorna 404 Not Found
				}
				await _sellerService.UpdateAsync(existingSeller); // Passar o vendedor existente atualizado
				return NoContent(); // Retorna 204 No Content
			}
			catch (DbUpdateConcurrencyException)
			{
				throw; // Deixe o Entity Framework lidar com a exceção de concorrência
			}
		}

		[HttpDelete("{id}")] // Excluir um vendedor
		[SwaggerOperation(Summary = "Excluir um vendedor")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _sellerService.RemoveAsync(id);
				return NoContent(); // Retorna 204 No Content
			}
			catch (IntegrityException e)
			{
				return StatusCode(500, e.Message); // Retorna 500 Internal Server Error
			}
		}
	}
}
