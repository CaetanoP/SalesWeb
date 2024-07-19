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

		public List<Seller> FindAll()
		{

			return _context.Seller.ToList();
		}

		public void Insert(Seller obj)
		{
			_context.Add(obj);
			_context.SaveChanges();
		}

		public Seller FindById(int id)
		{
			return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
		}
		public void Remove(int id)
		{
			var obj = _context.Seller.Find(id);
			_context.Seller.Remove(obj);
			_context.SaveChanges();
		}
		public void Update(Seller obj)
		{
			if (!_context.Seller.Any(x => x.Id == obj.Id))
			{
				throw new NotFoundException("Id not found");
			}
			try
			{
				_context.Update(obj);
				_context.SaveChanges();
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
