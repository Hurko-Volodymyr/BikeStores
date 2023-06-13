using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BikeStores.Models;
using BikeStores.Models.Enums;
using BikeStores.Services;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Order.Host.Configurations;

namespace BikeStores.Services.Tests
{
    [TestClass]
    public class OrderServiceTest
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<OrderService>> _logger;
        private readonly Mock<IInternalHttpClientService> _httpClient;
        private readonly Mock<IOptions<OrderConfig>> _config;


        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [TestMethod]
        public async Task CreateOrderAsync_PositiveTest()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);

                var customerId = 1;
                var orderStatus = OrderStatusEnum.Pending;
                var orderDate = DateTime.Now;
                var requiredDate = DateTime.Now.AddDays(7);
                var shippedDate = DateTime.Now.AddDays(1);
                var storeId = 1;
                var staffId = 1;
                var orderItems = new List<OrderItem>();

                // Act
                var orderId = await orderService.CreateOrderAsync(
                    customerId, orderStatus, orderDate, requiredDate, shippedDate, storeId, staffId, orderItems);

                // Assert
                Assert.IsTrue(orderId > 0, "Order ID should be greater than 0.");
            }
        }

        [TestMethod]
        public async Task CreateOrderAsync_NegativeTest()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);

                var customerId = 1;
                var orderStatus = 99;
                var orderDate = DateTime.Now;
                var requiredDate = DateTime.Now.AddDays(7);
                var shippedDate = DateTime.Now.AddDays(1);
                var storeId = 1;
                var staffId = 1;
                var orderItems = new List<OrderItem>();

                // Act & Assert
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                {
                    await orderService.CreateOrderAsync(
                        customerId, (OrderStatusEnum)orderStatus, orderDate, requiredDate, shippedDate, storeId, staffId, orderItems);
                });
            }
        }
        [TestMethod]
        public async Task GetOrderByIdAsync_PositiveTest()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);

                var customer = new Customer
                {
                    CustomerId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "123456789",
                    Email = "john@example.com",
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "12345"
                };
                dbContext.Customers.Add(customer);

                var store = new Store
                {
                    StoreId = 1,
                    StoreName = "Test Store",
                    Phone = "123456789",
                    Email = "test@store.com",
                    Street = "Test Street",
                    City = "Test City",
                    State = "TS",
                    ZipCode = "12345"
                };
                dbContext.Stores.Add(store);

                var staff = new Staff
                {
                    StaffId = 1,
                    FirstName = "Test Staff",
                    LastName = "Test",
                    Email = "test@example.com",
                    Phone = "123456789",
                    Active = 1,
                    StoreId = 1,
                    ManagerId = null,
                    Store = store,
                    Manager = null
                };
                dbContext.Staffs.Add(staff);

                var product = new Product
                {
                    ProductId = 1,
                    ProductName = "Test Product",
                    BrandId = 1,
                    CategoryId = 1,
                    ModelYear = 2023,
                    ListPrice = 9.99m
                };
                dbContext.Products.Add(product);

                var orderStatus = OrderStatusEnum.Pending;
                var orderDate = DateTime.Now;
                var requiredDate = DateTime.Now.AddDays(7);
                var shippedDate = DateTime.Now.AddDays(1);

                var orderItems = new List<OrderItem>
                {
                  new OrderItem
                  {
                   Product = product,
                   Quantity = 1,
                   ListPrice = 9.99m,
                   Discount = 0.00m
                   }
                };

                var orderId = await orderService.CreateOrderAsync(
                    customer.CustomerId, orderStatus, orderDate, requiredDate, shippedDate, store.StoreId, staff.StaffId, orderItems);
                await dbContext.SaveChangesAsync();

                // Act
                var result = await orderService.GetOrderByIdAsync(orderId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(orderId, result.OrderId);
                Assert.AreEqual(orderStatus, (OrderStatusEnum)result.OrderStatus);
                Assert.AreEqual(orderDate, result.OrderDate);
                Assert.AreEqual(requiredDate, result.RequiredDate);
                Assert.AreEqual(shippedDate, result.ShippedDate);
                Assert.AreEqual(orderItems.Count, result.OrderItems.Count);
            }
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_NegativeTest_InvalidOrderId()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);

                var invalidOrderId = -1;

                // Act
                var result = await orderService.GetOrderByIdAsync(invalidOrderId);

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task GetOrdersAsync_PositiveTest()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);

                var customer = new Customer
                {
                    CustomerId = 13,
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "123456789",
                    Email = "john@example.com",
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "12345"
                };
                dbContext.Customers.Add(customer);

                var store = new Store
                {
                    StoreId = 31,
                    StoreName = "Test Store",
                    Phone = "123456789",
                    Email = "test@store.com",
                    Street = "Test Street",
                    City = "Test City",
                    State = "TS",
                    ZipCode = "12345"
                };
                dbContext.Stores.Add(store);

                var staff = new Staff
                {
                    StaffId = 31,
                    FirstName = "Test Staff",
                    LastName = "Test",
                    Email = "test@example.com",
                    Phone = "123456789",
                    Active = 1,
                    StoreId = 1,
                    ManagerId = null,
                    Store = store,
                    Manager = null
                };
                dbContext.Staffs.Add(staff);

                var product = new Product
                {
                    ProductId = 31,
                    ProductName = "Test Product",
                    BrandId = 1,
                    CategoryId = 1,
                    ModelYear = 2023,
                    ListPrice = 9.99m
                };
                dbContext.Products.Add(product);

                var orderStatus = OrderStatusEnum.Pending;
                var orderDate = DateTime.Now;
                var requiredDate = DateTime.Now.AddDays(7);
                var shippedDate = DateTime.Now.AddDays(1);

                var orderItems = new List<OrderItem>
                {
                  new OrderItem
                  {
                   Product = product,
                   Quantity = 1,
                   ListPrice = 9.99m,
                   Discount = 0.00m
                   }
                };

                var page = 1;
                var pageSize = 10;

                var orderId = await orderService.CreateOrderAsync(
                    customer.CustomerId, orderStatus, orderDate, requiredDate, shippedDate, store.StoreId, staff.StaffId, orderItems);
                await dbContext.SaveChangesAsync();

                // Act
                var result = await orderService.GetOrdersAsync(page, pageSize);

                // Assert
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task GetOrdersAsync_NegativeTest_InvalidPageOrPageSize()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);
                var invalidPage = -1;
                var invalidPageSize = 0;

                // Act
                var result = await orderService.GetOrdersAsync(invalidPage, invalidPageSize);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public async Task CancelOrderAsync_PositiveTest()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);

                var order = new OrderEntity
                {
                    OrderId = 123,
                    OrderStatus = (byte)OrderStatusEnum.Pending,
                };

                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();

                // Act
                var result = await orderService.CancelOrderAsync(order.OrderId);

                // Assert
                Assert.IsTrue(result);

                var canceledOrder = await dbContext.Orders.FindAsync(order.OrderId);
                Assert.IsNotNull(canceledOrder);
                Assert.AreEqual((byte)OrderStatusEnum.Rejected, canceledOrder.OrderStatus);
            }
        }

        [TestMethod]
        public async Task CancelOrderAsync_NegativeTest()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var orderService = new OrderService(dbContext);

                var order = new OrderEntity
                {
                    OrderId = 121,
                    OrderStatus = 15,
                };

                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();

                // Act
                var result = await orderService.CancelOrderAsync(order.OrderId);

                // Assert

                var canceledOrder = await dbContext.Orders.FindAsync(order.OrderId);
                Assert.IsNotNull(canceledOrder);
                Assert.AreEqual((byte)OrderStatusEnum.Rejected, canceledOrder.OrderStatus);
            }
        }

    }

}


