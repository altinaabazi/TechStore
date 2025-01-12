using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechStore.Models;

namespace TechStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderStatus> orderStatuses { get; set; }
        public DbSet<Stock> Stocks { get; set; }
       
    }
}