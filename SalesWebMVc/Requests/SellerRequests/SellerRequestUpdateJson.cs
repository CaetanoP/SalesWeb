using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVc.Requests.SellerRequests
{
	public class SellerRequestUpdateJson
	{
		public String? Name { get; set; }
		public String? Email { get; set; }
		public double BaseSalary { get; set; }
		[SwaggerSchema(Format = "date")]
		public DateTime BirthDate { get; set; }
		public int DepartmentId { get; set; }


	}
}
