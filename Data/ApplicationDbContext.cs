using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ABCRetailers.Models;

namespace ABCRetailers.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        // DbSet for Products table
        public DbSet<Product> Products { get; set; }

        // DbSet for FileUploads table


        // DbSet for Carts table
        public DbSet<Cart> Carts { get; set; }

        // DbSet for CartItems table
        public DbSet<CartItem> CartItems { get; set; }

        // DbSet for Orders table
        public DbSet<Order> Orders { get; set; }

        // DbSet for OrderItems table
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Specify precision for decimal properties
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.TotalPrice)
                .HasPrecision(18, 2);
        }

    }
}
