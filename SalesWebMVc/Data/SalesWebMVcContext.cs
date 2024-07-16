using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Models;

namespace SalesWebMVc.Data
{
    public class SalesWebMVcContext : DbContext
    {
        public SalesWebMVcContext (DbContextOptions<SalesWebMVcContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Department { get; set; } = default!;
        public DbSet<Seller> Seller { get; set; } = default!;
        public DbSet<SalesRecord> SalesRecord { get; set; } = default!;
    }
}
