using SalesWebMVc.Data;
using SalesWebMVc.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Responses.DepartmentResponses;
using SalesWebMVc.Services.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using SalesWebMVc.Models.Validator;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;

namespace SalesWebMVc.Services
{
	public class DepartmentService
	{
		private readonly SalesWebMVcContext _context;
		private readonly DepartmentValidator _departmentValidator;
		public DepartmentService(SalesWebMVcContext context, DepartmentValidator departmentValidator)
		{
			_context = context;
			_departmentValidator = departmentValidator;
		}
		//Method to find all departments
		public async Task<List<DepartmentResponseJson>> FindAllAsync()
		{
			var departments = await _context.Department.OrderBy(x => x.Name).ToListAsync();
			if (departments.Count == 0)
				throw new NotFoundException("No departments found");

			return departments.Select(x => new DepartmentResponseJson
			{
				Id = x.Id,
				Name = x.Name
			}).ToList();
		}
		//Method to find a department by id
		public async Task<DepartmentResponseDetailJson> FindByIdAsync(int id)
		{
			//The default value of the department is null
			var department = await _context.Department.Include(d => d.Sellers).FirstOrDefaultAsync(x => x.Id == id);

			if (department is null)
				throw new NotFoundException("Department not found");

			return new DepartmentResponseDetailJson
			{
				Id = department.Id,
				Name = department.Name,
				Sellers = new List<Seller>(),
			};
		}
		//Method to create a department
		public async Task CreateAsync(Department department)
		{
			// Using FluentValidation to validate the department sintaxe
			var validationResult = await _departmentValidator.ValidateAsync(department);
			//veriy if the department id already exists
			bool hasAny = await _context.Department.AnyAsync(x => x.Name == department.Name);

			if (hasAny)
				throw new IntegrityException("There is already a department with this name");

			if (!validationResult.IsValid)
			{
				//If the validation is not valid, the method will throw a BadRequestException with the error messages
				var errors = validationResult.Errors.Select(e => e.ErrorMessage);
				throw new BadRequestException(string.Join(".\n", errors));
			}


			try
			{

				_context.Department.Add(department);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				throw new IntegrityException("Department Id conflict, try a different Id number");
			}
			catch (Exception)
			{
				throw new Exception("An error occurred while creating the department");
			}

		}
		//Method to update a department
		public async Task UpdateAsync(Department department)
		{
			//
			bool hasAny = await _context.Department.AnyAsync(x => x.Id == department.Id);
			if (!hasAny)
				throw new NotFoundException("Department not found");
			bool sameName = await _context.Department.AnyAsync(x => x.Name == department.Name && x.Id != department.Id);
			if (sameName)
				throw new IntegrityException("There is already a department with this name");

			try
			{
				_context.Update(department);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				throw new DbConcurrencyException(e.Message);
			}
			catch (Exception)
			{
				throw new Exception("An error occurred while updating the department");
			}

		}
		//Method to delete a department
		public async Task RemoveAsync(int id)
		{

			bool exists = await _context.Department.AnyAsync(d => d.Id == id);

			if (!exists)
			{
				throw new NotFoundException("Department not found");
			}

			try
			{
				var department = await _context.Department.FindAsync(id);
				_context.Department.Remove(department);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				throw new DbConcurrencyException(e.Message);
			}
			catch (DbUpdateException e)
			{
				throw new IntegrityException("Can't delete department because it has sellers");
			}
			catch (Exception)
			{
				throw new Exception("An error occurred while removing the department");
			}
		}
		//Method to get all the sellers from a department by id
		public async Task<List<Seller>> FindSellersAsync(int id)
		{
			try
			{
				var department = await _context.Department.Include(d => d.Sellers).FirstOrDefaultAsync(x => x.Id == id);
				if (department is null)
					throw new NotFoundException("Department not found");
				if (department.Sellers.IsNullOrEmpty())
					throw new NotFoundException("No sellers found for this department");
				return department.Sellers.ToList();
			}
			catch (NotFoundException e)
			{
				throw new NotFoundException(e.Message);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

		}
		//Method to get all sales from a department by id
		public async Task<List<SalesRecord>> FindSalesAsync(int id)
		{
			//Include is used to get the sellers from the department
			try
			{
				var department = await _context.Department.Include(d => d.Sellers).FirstOrDefaultAsync(x => x.Id == id);
				if (department is null)
					throw new NotFoundException("Department not found");

				var sellers = department.Sellers.ToList();
				List<SalesRecord> sales = new List<SalesRecord>();
				foreach (var seller in sellers)
				{
					var salesRecord = await _context.SalesRecord.Where(x => x.SellerId == seller.Id).ToListAsync();
					sales.AddRange(salesRecord);
				}
				if (sales.IsNullOrEmpty())
					throw new NotFoundException("No sales found for this department");

				return sales;
			}
			catch (NotFoundException e)
			{
				throw new NotFoundException(e.Message);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

		}
		//Method to get the total sales value from a department by id
		public async Task<string> TotalSalesAsync(int id)
		{
			try
			{
				var department = await _context.Department.Include(d => d.Sellers).FirstOrDefaultAsync(x => x.Id == id);
				if (department is null)
					throw new NotFoundException("Department not found");

				var sellers = department.Sellers.ToList();
				List<SalesRecord> sales = new List<SalesRecord>();
				foreach (var seller in sellers)
				{
					var salesRecord = await _context.SalesRecord.Where(x => x.SellerId == seller.Id).ToListAsync();
					sales.AddRange(salesRecord);
				}
				if(sales.IsNullOrEmpty())
					throw new NotFoundException("No sales found for this department");

				double totalSales = sales.Sum(x => x.Amount);
				CultureInfo cultureBrazil = new CultureInfo("pt-BR");
				string totalSalesFormatted = totalSales.ToString("C", cultureBrazil);
				return totalSalesFormatted;

			}
			catch (NotFoundException e)
			{
				throw new NotFoundException(e.Message);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		
	}
}
