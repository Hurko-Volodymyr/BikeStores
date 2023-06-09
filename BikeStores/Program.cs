using BikeStores.Models;
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
    var customers = db.Customers.ToList();

    var order = await orderService.GetOrderByIdAsync(1);
    
    
        Console.WriteLine($"Order ID: {order.OrderId}, {order.Customer.FirstName}, {order.Store.StoreName}, {order.Staff.FirstName}, Order items product name: {order.OrderItems.First().Product.ProductName}");


}