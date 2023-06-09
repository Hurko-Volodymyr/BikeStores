using BikeStores.Models;
using BikeStores.Models.Enums;

namespace BikeStores.Services.Abstractions
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(int customerId, OrderStatusEnum orderStatus, DateTime orderDate, DateTime requiredDate,
           DateTime? shippedDate, int storeId, int staffId, List<OrderItem> orderItems);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersAsync(int page, int pageSize);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
