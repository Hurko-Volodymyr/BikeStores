using BikeStores.Models.Enums;
using BikeStores.Models;

namespace Order.Host.Repositories.Abstractions
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(int customerId, OrderStatusEnum orderStatus, DateTime orderDate, DateTime requiredDate, DateTime? shippedDate, int storeId, int staffId, List<OrderItem> orderItems);
        Task<OrderEntity?> GetOrderByIdAsync(int orderId);
        Task<PaginatedItems<OrderEntity>?> GetOrdersAsync(int pageSize, int pageIndex);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
