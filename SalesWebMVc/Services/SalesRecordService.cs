using SalesWebMVc.Data;
using SalesWebMVc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Responses;
using SalesWebMVc.Services;
using SalesWebMVc.Services.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace SalesWebMVc.Services
{
	public class SalesRecordService
	{
		private readonly SalesWebMVcContext _context;
		private readonly SellerService _sellerService;

		public SalesRecordService(SalesWebMVcContext context, SellerService sellerService)
		{
			_context = context;
			_sellerService = sellerService;
		}

		public async Task<List<SalesRecord>> GetAllAsync(DateTime? minDate, DateTime? maxDate)
		{
			try
			{

				var result = await _context.SalesRecord
					.Where(x => x.Date >= minDate.Value && x.Date <= maxDate.Value)
					.OrderByDescending(x => x.Date)
					.ToListAsync();
				if(result.IsNullOrEmpty())
					throw new Exception("No Sales Records Found");

				return result;
			}
			catch (DbConcurrencyException)
			{
				throw new Exception("Error: Database Concurrency Exception");
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
			

			
		}



	}
}
