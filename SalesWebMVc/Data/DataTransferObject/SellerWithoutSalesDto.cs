using SalesWebMVc.Models;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVc.Data.DataTransferObject
{
	public class SellerWithoutSalesDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public double BaseSalary { get; set; }
		public DateTime BirthDate { get; set; }
		public int DepartmentId { get; set; }


        public SellerWithoutSalesDto(Seller seller)
        {
			seller.Id = Id;
			seller.Name = Name;
			seller.Email = Email;
			seller.BaseSalary = BaseSalary;
			seller.BirthDate = BirthDate;
			seller.DepartmentId = DepartmentId;
            
        }
    }
}
