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
		[SwaggerOperation(Summary = "Obter todos os departamentos")]
		[SwaggerResponse(200, "Departamentos encontrados")]
		[SwaggerResponse(404, "Nenhum departamento encontrado")]
		public async Task<ActionResult<IEnumerable<DepartmentResponseJson>>> GetAll()
		{
			var response =await _departmentService.FindAllAsync();

			return response;
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Summary = "Obter detalhes de um departamento")]
		[SwaggerResponse(200, "Departamento encontrado", typeof(DepartmentResponseJson))]
		[SwaggerResponse(404, "Departamento não encontrado")]
		public async Task<ActionResult<DepartmentResponseDetailJson>> Details(int id)
		{
			var response = await _departmentService.FindByIdAsync(id);

			return response;
		}

		[HttpPost]
		[SwaggerOperation(Summary = "Criar um novo departamento")]
		[SwaggerResponse(201, "Departamento criado", typeof(DepartmentRequestCreateJson))]
		[SwaggerResponse(400, "Dados do departamento errados")]

		public async Task<ActionResult<Department>> Create(DepartmentRequestCreateJson departmentRequest)
		{

			var department = new Department(departmentRequest.Name);

			await _departmentService.CreateAsync(department);

			return CreatedAtAction("GetDepartment", new { id = department.Id }, department);

		}

		[HttpPut]
		[SwaggerOperation(Summary = "Atualizar um departamento existente")]
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
		[SwaggerOperation(Summary = "Excluir um departamento")]
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
