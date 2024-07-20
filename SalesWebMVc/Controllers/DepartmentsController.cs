using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Data;
using SalesWebMVc.Models;
using Swashbuckle.AspNetCore.Annotations; // Importar para usar os atributos do Swagger

namespace SalesWebMVc.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DepartmentsController : ControllerBase
	{
		private readonly SalesWebMVcContext _context;

		public DepartmentsController(SalesWebMVcContext context)
		{
			_context = context;
		}

		[HttpGet]
		[SwaggerOperation(Summary = "Obter todos os departamentos")]
		public async Task<ActionResult<IEnumerable<Department>>> Index()
		{
			return await _context.Department.ToListAsync();
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Summary = "Obter detalhes de um departamento")]
		public async Task<ActionResult<Department>> Details(int id)
		{
			var department = await _context.Department.FindAsync(id);

			if (department == null)
			{
				return NotFound();
			}

			return department;
		}

		[HttpPost]
		[SwaggerOperation(Summary = "Criar um novo departamento")]
		public async Task<ActionResult<Department>> Create(Department department)
		{
			_context.Department.Add(department);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
		}

		[HttpPut("{id}")]
		[SwaggerOperation(Summary = "Atualizar um departamento existente")]
		public async Task<IActionResult> Edit(int id, Department department)
		{
			if (id != department.Id)
			{
				return BadRequest();
			}

			_context.Entry(department).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DepartmentExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		[SwaggerOperation(Summary = "Excluir um departamento")]
		public async Task<IActionResult> Delete(int id)
		{
			var department = await _context.Department.FindAsync(id);
			if (department == null)
			{
				return NotFound();
			}

			_context.Department.Remove(department);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool DepartmentExists(int id)
		{
			return _context.Department.Any(e => e.Id == id);
		}
	}
}
