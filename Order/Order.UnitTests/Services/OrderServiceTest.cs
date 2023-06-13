using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BikeStores.Models;
using BikeStores.Models.Enums;
using Order.Host.Configurations;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace BikeStores.Services.Tests
{
    [TestClass]
    public class OrderServiceTest
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private readonly IOrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<OrderService>> _logger;
        private readonly Mock<IInternalHttpClientService> _httpClient;
        private readonly Mock<IOptions<OrderConfig>> _config;
        public OrderServiceTest()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<OrderService>>();
            _httpClient = new Mock<IInternalHttpClientService>();
            _config = new Mock<IOptions<OrderConfig>>();

            _config.Setup(s => s.Value).Returns(new OrderConfig());

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _orderService = new OrderService(_dbContextWrapper.Object, _logger.Object, _orderRepository.Object, _logger.Object, _mapper.Object, _httpClient.Object, _config.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _config.Setup(s => s.Value).Returns(new OrderConfig());
            var loggerBaseDataService = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            var loggerOrderService = _logger.Object;
            var httpClient = new Mock<IInternalHttpClientService>();

        }


        [TestMethod]
        public async Task CreateOrderAsync_PositiveTest()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;

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

        [TestMethod]
        public async Task CreateOrderAsync_NegativeTest()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;

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

        [TestMethod]
        public async Task GetOrderByIdAsync_PositiveTest()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;

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

        [TestMethod]
        public async Task GetOrderByIdAsync_NegativeTest_InvalidOrderId()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;

            var invalidOrderId = -1;

            // Act
            var result = await orderService.GetOrderByIdAsync(invalidOrderId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetOrdersAsync_PositiveTest()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;

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
            var result = await orderService.GetOrdersAsync(pageSize, page);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetOrdersAsync_NegativeTest_InvalidPageOrPageSize()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;
            var invalidPage = -1;
            var invalidPageSize = 0;

            // Act
            var result = await orderService.GetOrdersAsync(invalidPageSize, invalidPage);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CancelOrderAsync_PositiveTest()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;

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

        [TestMethod]
        public async Task CancelOrderAsync_NegativeTest()
        {
            // Arrange
            var dbContext = new ApplicationDbContext(_options);
            var orderService = _orderService;

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
