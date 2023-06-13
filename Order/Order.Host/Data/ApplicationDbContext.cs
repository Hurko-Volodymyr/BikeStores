using BikeStores.Models;

namespace Order.Host.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = default!;

        // public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Store> Stores { get; set; } = default!;
        public DbSet<Staff> Staffs { get; set; } = default!;
        public DbSet<OrderEntity> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;

        // public DbSet<Stock> Stocks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
          .HasKey(o => new { o.OrderId, o.ItemId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
