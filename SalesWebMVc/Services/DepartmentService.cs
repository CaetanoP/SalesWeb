using SalesWebMVc.Data;
using SalesWebMVc.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Responses.DepartmentResponses;
using SalesWebMVc.Services.Exceptions;

namespace SalesWebMVc.Services
{
	public class DepartmentService
	{
		private readonly SalesWebMVcContext _context;
		public DepartmentService(SalesWebMVcContext context)
		{
			_context = context;
		}
		//Method to find all departments
		public async Task<List<DepartmentResponseJson>> FindAllAsync()
		{
			var departments = await _context.Department.OrderBy(x => x.Name).ToListAsync();

			return departments.Select(x => new DepartmentResponseJson
			{
				Id = x.Id,
				Name = x.Name
			}).ToList();
		}
		//Method to find a department by id
		public async Task<DepartmentResponseJson> FindByIdAsync(int id)
		{
			var department = await _context.Department.FirstOrDefaultAsync(x => x.Id == id);

			if(department == null)
			{
				return null;
			}
			return new DepartmentResponseJson
			{
				Id = department.Id,
				Name = department.Name
			};
		}
		//Method to create a department
		public async Task CreateAsync(Department department)
		{
			_context.Department.Add(department);
			await _context.SaveChangesAsync();
		}
		//Method to update a department
		public async Task UpdateAsync(Department department)
		{
			bool hasAny = await _context.Department.AnyAsync(x => x.Id == department.Id);

			if(!hasAny)
			{
				throw new NotFoundException("Department not found");
			}
			try
			{
				_context.Update(department);
				await _context.SaveChangesAsync();
			}
			catch(DbUpdateConcurrencyException e)
			{
				//the DbUpdateConcurrencyException is a exception that is thrown when a concurrency error is encountered while saving to the database
				//that means that another user has updated the data in the database since the data was loaded
				throw new DbConcurrencyException(e.Message);
			}
		}
		//Method to delete a department
		public async Task RemoveAsync(int id)
		{
			try
			{
				var department = await _context.Department.FindAsync(id);
				_context.Department.Remove(department);
				await _context.SaveChangesAsync();
			}
			catch(DbUpdateException e)
			{
				//the DbUpdateException is a exception that is thrown when an error is encountered while saving to the database
				throw new IntegrityException(e.Message);
			}
		}
	}
}
