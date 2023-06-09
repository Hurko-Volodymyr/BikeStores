using BikeStores.Models;
using BikeStores.Models.Enums;
using BikeStores.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BikeStores.Services
{
    public class OrderService : IOrderService
    {

        private readonly ApplicationContext _dbContext;

        public OrderService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

public async Task<int> CreateOrderAsync(int customerId, OrderStatusEnum orderStatus, DateTime orderDate, DateTime requiredDate,
    DateTime? shippedDate, int storeId, int staffId, List<OrderItem> orderItems)
{
    if (!Enum.IsDefined(typeof(OrderStatusEnum), orderStatus))
    {
        throw new ArgumentException("Invalid OrderStatusEnum value.");
    }

    var order = new Order
    {
        CustomerId = customerId,
        OrderStatus = (byte)orderStatus,
        OrderDate = orderDate,
        RequiredDate = requiredDate,
        ShippedDate = shippedDate,
        StoreId = storeId,
        StaffId = staffId,
        OrderItems = orderItems
    };

    _dbContext.Orders.Add(order);
    await _dbContext.SaveChangesAsync();

    return order.OrderId;
}





        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Staff)
                .Include(i => i.OrderItems)
                    .ThenInclude(ti => ti.Product)
                .FirstOrDefaultAsync(f => f.OrderId == orderId);

            return order;
        }
        public async Task<List<Order>> GetOrdersAsync(int page, int pageSize)
        {
            var orders = await _dbContext.Orders
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Staff)
                .Include(i => i.OrderItems)
                    .ThenInclude(ti => ti.Product)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return orders;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            var isCanceled = false;

            if (order != null)
            {
                if (Enum.IsDefined(typeof(OrderStatusEnum), OrderStatusEnum.Rejected))
                {
                    order.OrderStatus = (byte)OrderStatusEnum.Rejected;
                    await _dbContext.SaveChangesAsync();
                    isCanceled = true;
                }
                else
                {
                    throw new ArgumentException("Invalid OrderStatusEnum value.");
                }
            }

            return isCanceled;
        }

    }
}
