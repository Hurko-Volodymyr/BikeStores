using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStores.Services.Abstractions
{
    internal interface IOrderService
    {
        Task<int> CreateOrderAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersAsync(int page, int pageSize);
        Task CancelOrderAsync(int orderId);
    }
}
