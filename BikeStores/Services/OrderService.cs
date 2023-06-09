using BikeStores.Models;
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
            var order = await _dbContext.Orders
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Staff)
                .Include(i => i.OrderItems)
                    .ThenInclude(ti => ti.Product)
                .FirstOrDefaultAsync(f => f.OrderId == orderId);

            return order;
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
