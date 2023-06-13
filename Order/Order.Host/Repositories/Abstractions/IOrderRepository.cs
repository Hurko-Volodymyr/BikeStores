using BikeStores.Models.Enums;
using BikeStores.Models;
using Order.Host.Models;

namespace Order.Host.Repositories.Abstractions
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(int customerId, OrderStatusEnum orderStatus, DateTime orderDate, DateTime requiredDate, DateTime? shippedDate, int storeId, int staffId, List<OrderItem> orderItems);
        Task<OrderEntity?> GetOrderByIdAsync(int orderId);
        Task<List<OrderEntity>?> GetOrdersAsync(int page, int pageSize);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
