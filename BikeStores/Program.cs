using BikeStores.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json");
var config = builder.Build();
var connectionString = config.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
var options = optionsBuilder
    .UseSqlServer(connectionString)
    .Options;



using (var db = new ApplicationContext(options))
{
    var orderService = new OrderService(db);
    //var orders = db.Orders.ToList();

    var order = await orderService.GetOrderByIdAsync(1);
    //        foreach (var order in orders)
    
        Console.WriteLine($"Order ID: {order.OrderId}, Order Status: {order.OrderStatus}, Order Date: {order.OrderDate}");
    
}
