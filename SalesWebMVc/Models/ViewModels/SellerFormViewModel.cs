namespace SalesWebMVc.Models.ViewModels
{
	public class SellerFormViewModel
	{
		//This class is a ViewModel that will be used to pass the data to the view
		public Seller Seller { get; set; }
		public ICollection<Department> Departments { get; set; }
	}
}
