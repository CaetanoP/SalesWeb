using SalesWebMVc.Models;

namespace SalesWebMVc.Requests.DepartmentsRequests
{
    public class DepartmentRequestDetailJson
    {
        int Id { get; set; } = 0;
        string? Name { get; set; }
        ICollection<Seller> Sellers { get; set; } = new List<Seller>();
    }
}
