using SalesWebMVc.Models;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVc.Responses.SellerResponses
{
	public class SellerResponseDetailJson
	{
		//This class is responsible for the response of the Seller Details
		public int Id { get;}
        public string Name { get;}
        public string Email { get;}
		public double BaseSalary { get;}
		public DateTime BirthDate { get;}
        public int DepartmentId { get;}
		public SellerResponseDetailJson(Seller seller)
		{
			Id = seller.Id;
			Name = seller.Name;
			Email = seller.Email;
			BaseSalary = seller.BaseSalary;
			//format the BirthDate to dd/MM/yyyy
			BirthDate = seller.BirthDate;
			BirthDate.ToString("dd/MM/yyyy");
			DepartmentId = seller.DepartmentId;
		}

	}

}
