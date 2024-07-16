namespace SalesWebMVc.Models
{
    public class Department
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();


        public Department()
        {
            
        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }
        public double TotalSales(DateTime initial, DateTime final)
        {
            //Using linq to filter the sales between the initial and final dates and sum the amount of each sale
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
