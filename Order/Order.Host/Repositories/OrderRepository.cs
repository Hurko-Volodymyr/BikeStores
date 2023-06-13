using BikeStores.Models;
using BikeStores.Models.Enums;

namespace Order.Host.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        {
            _dbContext = dbContextWrapper.DbContext;
        }

        public async Task<int?> CreateOrderAsync(int customerId, OrderStatusEnum orderStatus, DateTime orderDate, DateTime requiredDate, DateTime? shippedDate, int storeId, int staffId)
        {
            if (!Enum.IsDefined(typeof(OrderStatusEnum), orderStatus))
            {
                throw new ArgumentException("Invalid OrderStatusEnum value.");
            }

            var order = new OrderEntity
            {
                CustomerId = customerId,
                OrderStatus = (byte)orderStatus,
                OrderDate = orderDate,
                RequiredDate = requiredDate,
                ShippedDate = shippedDate,
                StoreId = storeId,
                StaffId = staffId,
            };

            var newItem = await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return newItem.Entity.OrderId;
        }

        public async Task<OrderEntity?> GetOrderByIdAsync(int orderId)
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

        public async Task<PaginatedItems<OrderEntity>?> GetOrdersAsync(int pageSize, int pageIndex)
        {
            IQueryable<OrderEntity> query = _dbContext.Orders;
            var orders = await _dbContext.Orders
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Staff)
                .Include(i => i.OrderItems)
                .ThenInclude(ti => ti.Product)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var totalItems = await query.LongCountAsync();

            return new PaginatedItems<OrderEntity>() { TotalCount = totalItems, Data = orders };
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