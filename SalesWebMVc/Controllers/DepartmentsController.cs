using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Data;
using SalesWebMVc.Models;
using SalesWebMVc.Responses.DepartmentResponses;
using SalesWebMVc.Requests;
using Swashbuckle.AspNetCore.Annotations; // Importar para usar os atributos do Swagger
using SalesWebMVc.Services;

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
		[SwaggerResponse(200, "Departamentos encontrados", typeof(DepartmentResponseJson))]
		[SwaggerResponse(404, "Nenhum departamento encontrado")]
		public async Task<ActionResult<IEnumerable<DepartmentResponseJson>>> Index()
		{
			var response =await _departmentService.FindAllAsync();

			return response;
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Summary = "Obter detalhes de um departamento")]
		[SwaggerResponse(200, "Departamento encontrado", typeof(DepartmentResponseJson))]
		[SwaggerResponse(404, "Departamento não encontrado")]
		public async Task<ActionResult<DepartmentResponseJson>> Details(int id)
		{
			var response = await _departmentService.FindByIdAsync(id);

			return response;
		}

		[HttpPost]
		[SwaggerOperation(Summary = "Criar um novo departamento")]
		[SwaggerResponse(201, "Departamento criado", typeof(DepartmentResponseJson))]
		public async Task<ActionResult<Department>> Create(DepartmentRequestJson departmentRequest)
		{
			var department = new Department(departmentRequest.Id, departmentRequest.Name);

			await _departmentService.CreateAsync(department);

			return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
		}

		[HttpPut]
		[SwaggerOperation(Summary = "Atualizar um departamento existente")]
		[SwaggerResponse(204, "Departamento atualizado")]
		[SwaggerResponse(400, "Id do departamento inválido")]
		public async Task<IActionResult> Edit(DepartmentRequestJson departmentRequest)
		{
			var department = new Department(departmentRequest.Id, departmentRequest.Name);
			await _departmentService.UpdateAsync(department);
			return NoContent();
		}

		[HttpDelete("{id}")]
		[SwaggerOperation(Summary = "Excluir um departamento")]
		[SwaggerResponse(204, "Departamento excluído")]
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
