using BikeStores.Models.Enums;
using BikeStores.Models;
using Catalog.Host.Models.Response.Items;

namespace Order.Host.Services.Abstractions
{
    public interface IOrderService
    {
        Task<int?> CreateOrderAsync(int customerId, OrderStatusEnum orderStatus, DateTime orderDate, DateTime requiredDate, DateTime? shippedDate, int storeId, int staffId);
        Task<OrderEntity?> GetOrderByIdAsync(int orderId);
        Task<PaginatedItemsResponse<OrderEntity>?> GetOrdersAsync(int pageSize, int pageIndex);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
