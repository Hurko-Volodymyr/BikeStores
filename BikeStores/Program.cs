﻿using BikeStores.Models;
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
    
    
        Console.WriteLine($"Order ID: {order.OrderId}, Order Status: {order.OrderStatus}, {order.Customer.FirstName}");

    //foreach (var customer in customers)
    //{
    //    if (customer != null)
    //    {
    //        Console.WriteLine($"Customer ID: {customer.CustomerId}");
    //        Console.WriteLine($"First Name: {customer.FirstName}");
    //        Console.WriteLine($"Last Name: {customer.LastName}");
    //    }
    //}
}
//Customer: { order.Customer.FirstName}