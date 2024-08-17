using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Data;
using SalesWebMVc.Models;
using SalesWebMVc.Responses.DepartmentResponses;
using SalesWebMVc.Requests;
using Swashbuckle.AspNetCore.Annotations; // Importar para usar os atributos do Swagger
using SalesWebMVc.Services;
using SalesWebMVc.Requests.DepartmentsRequests;

namespace SalesWebMVc.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DepartmentsController : ControllerBase
	{
		private readonly SalesWebMVcContext _context;
		private readonly DepartmentService _departmentService;
		public DepartmentsController(SalesWebMVcContext context, DepartmentService departmentService)
		{
			_context = context;
			_departmentService = departmentService;
		}

		[HttpGet]
		[SwaggerOperation(
			Summary = "Obter todos os departamentos",
			Description = "Retorna uma lista de todos os departamentos cadastrados. Se não houver departamentos, retorna uma lista vazia."
		)]
		[SwaggerResponse(200, "Departamentos encontrados")]
		[SwaggerResponse(404, "Nenhum departamento encontrado")]
		public async Task<ActionResult<IEnumerable<DepartmentResponseJson>>> GetAll()
		{
			var response = await _departmentService.FindAllAsync();

			return response;
		}

		[HttpGet("{id}")]
		[SwaggerOperation(
			Summary = "Obter detalhes de um departamento",
			Description = "Retorna os detalhes do departamento com o ID especificado."
		)]
		[SwaggerResponse(200, "Departamento encontrado", typeof(DepartmentResponseJson))]
		[SwaggerResponse(404, "Departamento não encontrado")]
		public async Task<ActionResult<DepartmentResponseDetailJson>> Details(int id)
		{
			var response = await _departmentService.FindByIdAsync(id);

			return response;
		}
		
		[HttpGet("{id}/sellers")]
		[SwaggerOperation(
			Summary = "Obter todos os vendedores de um departamento",
			Description = "Retorna todos os vendedores do departemento pelo id"
		)]
		[SwaggerResponse(200, "Vendedores encontrados", typeof(IEnumerable<Seller>))]
		[SwaggerResponse(404, "Nenhum vendedor encontrado")]
		public async Task<ActionResult<IEnumerable<Seller>>> GetSellers(int id)
		{
			var response = await _departmentService.FindSellersAsync(id);

			return response;
		}

		[HttpGet("{id}/sales")]
		[SwaggerOperation(
			Summary = "Obter todas as vendas de um departamento",
			Description = "Retorna todas as vendas do departamento pelo id"
		)]
		[SwaggerResponse(200, "Vendas encontradas", typeof(IEnumerable<SalesRecord>))]
		[SwaggerResponse(404, "Nenhuma venda encontrada")]
		public async Task<ActionResult<IEnumerable<SalesRecord>>> GetSales(int id)
		{
			var response = await _departmentService.FindSalesAsync(id);

			return response;
		}

		[HttpGet("{id}/total-sales")]
		[SwaggerOperation(
			Summary = "Obter o total vendido pelo departamento",
			Description = "Retorna o total em reais de vendas do departamento pelo id"
		)]
		[SwaggerResponse(200, "Total de vendas encontrado", typeof(string))]
		[SwaggerResponse(404, "Nenhuma venda encontrada")]
		public async Task<ActionResult<string>> GetTotalSales(int id)
		{
			var response = await _departmentService.TotalSalesAsync(id);

			return response;
		}
		[HttpGet("{id}/cost")]
		[SwaggerOperation(
			Summary = "Obter o custo total do departamento",
			Description = "Retorna o custo total em reais do departamento pelo id"
		)]
		[SwaggerResponse(200, "Custo total encontrado", typeof(string))]
		[SwaggerResponse(404, "Nenhum custo encontrado")]
		public async Task<ActionResult<string>> GetCost(int id)
		{
			var response = await _departmentService.CostAsync(id);

			return response;
		}

		[HttpPost]
		[SwaggerOperation(
			Summary = "Criar um novo departamento",
			Description = "Cria um novo departamento com os dados fornecidos no corpo da requisição. Retorna o departamento criado, incluindo o ID gerado automaticamente."
		)]
		[SwaggerResponse(201, "Departamento criado", typeof(DepartmentRequestCreateJson))]
		[SwaggerResponse(400, "Dados do departamento errados")]

		public async Task<ActionResult<Department>> Create(DepartmentRequestCreateJson departmentRequest)
		{

			var department = new Department(departmentRequest.Name);

			await _departmentService.CreateAsync(department);

			return CreatedAtAction("GetDepartment", new { id = department.Id }, department);

		}

		[HttpPut]
		[SwaggerOperation(
			Summary = "Atualizar um departamento existente",
			Description = "Atualiza os dados do departamento com o ID especificado no corpo da requisição. Retorna 204 No Content se a atualização for bem-sucedida."
		)]
		[SwaggerResponse(204, "Departamento atualizado")]
		[SwaggerResponse(400, "Bad request")]
		[SwaggerResponse(404, "Departamento não encontrado")]
		public async Task<IActionResult> Update(DepartmentRequestUpdateJson departmentRequest)
		{
			var department = new Department(departmentRequest.Id, departmentRequest.Name);
			await _departmentService.UpdateAsync(department);
			return NoContent();
		}

		[HttpDelete("{id}")]
		[SwaggerOperation(
			Summary = "Excluir um departamento",
			Description = "Exclui o departamento com o ID especificado. Retorna 204 No Content se a exclusão for bem-sucedida."
		)]
		[SwaggerResponse(204, "Departamento excluído")]
		[SwaggerResponse(404, "Departamento não encontrado")]
		public async Task<IActionResult> Delete(int id)
		{
			await _departmentService.RemoveAsync(id);
			return NoContent();
		}

		private bool DepartmentExists(int id)
		{
			return _context.Department.Any(e => e.Id == id);
		}
	}
}
