using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public DbSet<SalesWebMVc.Models.Department> Department { get; set; } = default!;
    }
}
