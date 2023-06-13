using BikeStores.Models;
using BikeStores.Models.Enums;
using Catalog.Host.Models.Response.Items;
using Microsoft.Extensions.Options;
using Order.Host.Configurations;
using Order.Host.Models.Dtos;

namespace Order.Host.Services
{
    public class OrderService : BaseDataService<ApplicationDbContext>, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _loggerService;
        private readonly IOrderRepository _orderRepository;
        private readonly IInternalHttpClientService _httpClient;
        private readonly OrderConfig _config;

        public OrderService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IOrderRepository orderRepository,
            ILogger<OrderService> loggerService,
            IMapper mapper,
            IInternalHttpClientService httpClient,
            IOptions<OrderConfig> config)
            : base(dbContextWrapper, logger)
        {
            _loggerService = loggerService;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _config = config.Value;
        }

        public async Task<int> CreateOrderAsync(int customerId, OrderStatusEnum orderStatus, DateTime orderDate, DateTime requiredDate, DateTime? shippedDate, int storeId, int staffId, List<OrderItem> orderItems)
        {
            var orderId = await _orderRepository.CreateOrderAsync(customerId, orderStatus, orderDate, requiredDate, shippedDate, storeId, staffId, orderItems);
            if (orderId! == default)
            {
                _loggerService.LogWarning($"Can`t adding order");
            }

            return orderId;
        }

        public async Task<OrderEntity?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order! == null)
            {
                _loggerService.LogWarning($"Not founded order");
                return null!;
            }

            return order;
        }

        public async Task<PaginatedItemsResponse<OrderEntity>?> GetOrdersAsync(int pageSize, int pageIndex)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _orderRepository.GetOrdersAsync(pageIndex, pageSize);
                if (result == null)
                {
                    _loggerService.LogWarning($"Orders not found");
                    return null;
                }

                return new PaginatedItemsResponse<OrderEntity>()
                {
                    Count = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<OrderEntity>(s)).ToList(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            });
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var isCanceled = await _orderRepository.CancelOrderAsync(orderId);

            if (isCanceled == false)
            {
                _loggerService.LogWarning($"Not founded order to cancel");
            }

            return isCanceled;
        }
    }
}
