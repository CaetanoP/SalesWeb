using SalesWebMVc.Data;
using SalesWebMVc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Responses;
using SalesWebMVc.Services;
using SalesWebMVc.Services.Exceptions;
using Microsoft.IdentityModel.Tokens;
using SalesWebMVc.Requests.SalesRequests;
using SalesWebMVc.Models.Validator;

namespace SalesWebMVc.Services
{
	public class SalesRecordService
	{
		private readonly SalesWebMVcContext _context;
		private readonly SellerService _sellerService;
		private readonly SalesRecordValidator _salesRecordValidator;

		public SalesRecordService(SalesWebMVcContext context, SellerService sellerService, SalesRecordValidator salesRecordValidator)
		{
			_context = context;
			_sellerService = sellerService;
			_salesRecordValidator = salesRecordValidator;
		}

		public async Task<List<SalesRecord>> GetAllAsync(DateTime? minDate, DateTime? maxDate)
		{
			try
			{

				var result = await _context.SalesRecord
					.Where(x => x.Date >= minDate.Value && x.Date <= maxDate.Value)
					.OrderByDescending(x => x.Date)
					.ToListAsync();
				if (result.IsNullOrEmpty())
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

		public async Task InsertAsync(SalesRecord sale)
		{
			Seller seler = await _sellerService.FindByIdAsync(sale.SellerId);
			if (seler == null)
				throw new NotFoundException("Seller not found");

			var validationResult = await _salesRecordValidator.ValidateAsync(sale);
			if (!validationResult.IsValid)
			{
				var errors = validationResult.Errors.Select(e => e.ErrorMessage);
				throw new BadRequestException(string.Join(".\n", errors));
			}
			try
			{

				_context.SalesRecord.Add(sale);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				throw new IntegrityException(e.Message);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		public async Task RemoveByIdAsnc(int id)
		{
				var sale = await _context.SalesRecord.FindAsync(id);
				if (sale is null)
					throw new NotFoundException("Sale not found");

			try
			{
				_context.SalesRecord.Remove(sale);
				await _context.SaveChangesAsync();
			}
			catch(DbConcurrencyException)
			{
				throw new DbConcurrencyException("Error: Database Concurrency Exception");
			}
			catch (DbUpdateException e)
			{
				throw new IntegrityException(e.Message);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

		}
		public async Task<SalesRecord> GetByIdAsync(int id)
		{
			var sale = await _context.SalesRecord.FindAsync(id);
			if (sale is null)
				throw new NotFoundException("Sale not found");

			return sale;
		}
		
	}
}
