using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVc.Requests.SellerRequests
{
	public class SellerRequestCreateJson
	{

		public string? Name { get; set; }
		public string? Email { get; set; }
		public double BaseSalary { get; set; }
		[SwaggerSchema(Format = "date")]
		public DateTime BirthDate { get; set; }
		public int DepartmentId { get; set; }

	}
}
