using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HomeDecoration.Models.DTO;

namespace HomeDecoration.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
        {
            public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
            {}
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
    
}
