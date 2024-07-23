using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models;

namespace SalesWebMVc.Responses.SellerResponses
{
	public class SellerResponseCreateJson 
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public double BaseSalary { get; set; }
		public DateTime BirthDate { get; set; }
		public int DepartmentId { get; set; }

		public SellerResponseCreateJson(int id, string name, string email, double baseSalary, DateTime birthDate, int departmentId)
		{
			Id = id;
			Name = name;
			Email = email;
			BaseSalary = baseSalary;
			BirthDate = birthDate;
			DepartmentId = departmentId;
		}
	}
}
