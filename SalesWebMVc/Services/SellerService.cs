using SalesWebMVc.Data;
using SalesWebMVc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Services.Exceptions;
using SalesWebMVc.Models.Validator;
using Microsoft.IdentityModel.Tokens;

namespace SalesWebMVc.Services
{
	public class SellerService
	{

		//Seller service is responsible for the Business Rules of the Seller
		//Interacts with the database, in this case the SalesWebMVcContext
		//Using a Microsoft Entity Framework Core, that is a ORM (Object Relational Mapping)
		//The CRUD operations are implemented here (Create, Read, Update, Delete)
		private readonly SalesWebMVcContext _context;
		private readonly SellerValidator _sellerValidator;

		public SellerService(SalesWebMVcContext context, SellerValidator sellerValidator)
		{
			_context = context;
			_sellerValidator = sellerValidator;
		}

		public async Task<List<Seller>> FindAllAsyncWithDetails()
		{
			var sellers = await _context.Seller.Include(x=> x.Sales).ToListAsync();
			if(sellers.IsNullOrEmpty())
			{
				throw new NotFoundException("No sellers found");
			}
			return sellers;
		
		}
		public async Task<List<Seller>> FindAllAsyncWithoutDetails()
		{
			var sellers = await _context.Seller.ToListAsync();
			if (sellers.IsNullOrEmpty())
			{
				throw new NotFoundException("No sellers found");
			}
			return sellers;

		}

		public async Task InsertAsync(Seller seller)
		{
			//Use validator to validate the Seller
			var validationResult = await _sellerValidator.ValidateAsync(seller);
			if(!validationResult.IsValid)
			{
				//If the validation is not valid, the method will throw a BadRequestException with the error messages
				var errors = validationResult.Errors.Select(e => e.ErrorMessage);
				throw new BadRequestException(string.Join(".\n", errors));
			}
			//Add a new Seller to the database
			try
			{
				_context.Add(seller);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				throw new IntegrityException(e.Message);
			}
			catch (Exception)
			{
				throw new Exception("An error occurred while creating the department");
			}

		}

		public async Task<Seller> FindByIdAsync(int id)
		{
			var response = await _context.Seller.FirstOrDefaultAsync(obj => obj.Id == id);
			if (response is null)
				throw new NotFoundException("Seller not found");
	
			return response;
		}
		public async Task RemoveAsync(int id)
		{
			
			try
			{
				var seller = await FindByIdAsync(id);
				_context.Seller.Remove(seller);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				throw new DbConcurrencyException(e.Message);
			}
			catch(IntegrityException e)
			{
				throw new IntegrityException(e.Message);
			}
			
			catch (DbUpdateException e)
			{
				throw new IntegrityException(e.Message);
			}
			catch(NotFoundException)
			{
				throw new NotFoundException("Seller not found");
			}
			catch (Exception)
			{
				throw new Exception("An error occurred while removing the seller");
			}
		}
		public async Task UpdateAsync(Seller seller)
		{
			//verify if the Id(seller) exists
			bool hasAny = await _context.Seller.AnyAsync(x => x.Id == seller.Id);
			if (!hasAny)
				throw new NotFoundException("Id not found");

			//verify if the the DepartmentId exists
			bool hasAnyDepartment = await _context.Department.AnyAsync(x => x.Id == seller.DepartmentId);
			if (!hasAnyDepartment)
				throw new NotFoundException("DepartmentId not found");

			//using the FluentValidation to validate the Seller sintax
			var validationResult = await _sellerValidator.ValidateAsync(seller);
			if (!validationResult.IsValid)
			{
				//If the validation is not valid, the method will throw a BadRequestException with the error messages
				var errors = validationResult.Errors.Select(e => e.ErrorMessage);
				throw new BadRequestException(string.Join(".\n", errors));
			}

			try
			{
				_context.Update(seller);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				//This exception is thrown when there is a concurrency problem
				//A concurrency problem is when two threads try to update the same data at the same time
				throw new DbConcurrencyException(e.Message);

			}
			catch (Exception)
			{
				throw new Exception("An error occurred while creating the department");
			}
		}

	}
}
