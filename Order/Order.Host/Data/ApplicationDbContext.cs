using BikeStores.Models;

namespace Order.Host.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        // public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // public DbSet<Stock> Stocks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
          .HasKey(o => new { o.OrderId, o.ItemId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
