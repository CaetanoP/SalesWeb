using SalesWebMVc.Data;
using SalesWebMVc.Models;

namespace SalesWebMVc.Services
{
    public class SellerService
    {
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
            //Provisory implementation
            obj.Department = _context.Department.First();


			_context.Add(obj);
			_context.SaveChanges();
		}

    }
}
