using SalesWebMVc.Data;
using SalesWebMVc.Models;

namespace SalesWebMVc.Services
{
	public class DepartmentService
	{
		private readonly SalesWebMVcContext _context;
		public DepartmentService(SalesWebMVcContext context)
		{
			_context = context;
		}
		public List<Department> FindAll()
		{
			return _context.Department.OrderBy(x => x.Name).ToList();
		}
	}
}
