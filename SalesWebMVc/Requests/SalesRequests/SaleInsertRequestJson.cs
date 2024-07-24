using SalesWebMVc.Models.Enums;

namespace SalesWebMVc.Requests.SalesRequests
{
	public class SaleInsertRequestJson
	{
		public double Amount { get; set; }
		public SaleStatus Status { get; set; }
		public int SellerId { get; set; }
    }
}
