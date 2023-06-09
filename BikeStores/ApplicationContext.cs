using BikeStores.Models;
using Microsoft.EntityFrameworkCore;

internal class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    //public DbSet<Brand> Brands { get; set; }
    //public DbSet<Product> Products { get; set; }
    //public DbSet<Customer> Customers { get; set; }
    //public DbSet<Store> Stores { get; set; }
    //public DbSet<Staff> Staffs { get; set; }
    public DbSet<Order> Orders { get; set; }
    //public DbSet<OrderItem> OrderItems { get; set; }
    //public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}