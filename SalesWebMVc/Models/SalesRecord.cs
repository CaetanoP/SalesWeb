using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVc.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }


		[SwaggerSchema(Format = "date")]
		public DateTime Date { get; set; }


        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Amount { get; set; }
        public SaleStatus Status { get; set; }
        public int SellerId { get; set; }

        public SalesRecord()
        {
            
        }

        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, int sellerId)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            SellerId = sellerId;

        }
    }
}
