using SalesWebMVc.Data;
using SalesWebMVc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Services.Exceptions;

namespace SalesWebMVc.Services
{
	public class SellerService
	{

		//Seller service is responsible for the Business Rules of the Seller
		//Interacts with the database, in this case the SalesWebMVcContext
		//Using a Microsoft Entity Framework Core, that is a ORM (Object Relational Mapping)
		//The CRUD operations are implemented here (Create, Read, Update, Delete)
		private readonly SalesWebMVcContext _context;
		public SellerService(SalesWebMVcContext context)
		{
			_context = context;
		}

		public async Task<List<Seller>> FindAllAsync()
		{

			return await _context.Seller.ToListAsync();
		}

		public async Task InsertAsync(Seller obj)
		{
			_context.Add(obj);
			await _context.SaveChangesAsync();
		}

		public async Task<Seller> FindByIdAsync(int id)
		{
			return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
		}
		public async Task RemoveAsync(int id)
		{
			var obj = await _context.Seller.FindAsync(id);
			_context.Seller.Remove(obj);
			_context.SaveChangesAsync();
		}
		public async Task UpdateAsync(Seller obj)
		{
			bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
			if (!hasAny)
			{
				throw new NotFoundException("Id not found");
			}
			try
			{
				_context.Update(obj);
				await _context.SaveChangesAsync();
			}
			catch(DbUpdateConcurrencyException e)
			{
				//This exception is thrown when there is a concurrency problem
				//A concurrency problem is when two threads try to update the same data at the same time
				throw new DbConcurrencyException(e.Message);

			}
		}

	}
}
