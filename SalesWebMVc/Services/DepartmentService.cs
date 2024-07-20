using SalesWebMVc.Data;
using SalesWebMVc.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMVc.Services
{
	public class DepartmentService
	{
		private readonly SalesWebMVcContext _context;
		public DepartmentService(SalesWebMVcContext context)
		{
			_context = context;
		}
		public async Task<List<Department>> FindAllAsync()
		{
			return await _context.Department.OrderBy(x => x.Name).ToListAsync();
		}
	}
}
