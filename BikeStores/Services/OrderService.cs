using BikeStores.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStores.Services
{
    internal class OrderService : IOrderService
    {

        private readonly ApplicationContext _dbContext;

        public OrderService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> CreateOrderAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(f => f.OrderId == orderId);
        }

        public Task<List<Order>> GetOrdersAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task CancelOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
