using SalesWebMVc.Models;

namespace SalesWebMVc.Responses.DepartmentResponses
{
	public class DepartmentResponseDetailJson
	{
		public int Id { get; set; } = 0;
		public string Name { get; set; }
		public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

	}
}
