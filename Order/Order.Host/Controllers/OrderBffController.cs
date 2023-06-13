using Catalog.Host.Models.Requests.Items;
using Catalog.Host.Models.Response.Items;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Order.Host.Models.Dtos;

namespace Order.Host.Controllers
{
    [ApiController]
    [Scope("order.orderbff")]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderBffController : ControllerBase
    {
        private readonly ILogger<OrderBffController> _logger;
        private readonly IOrderService _orderService;

        public OrderBffController(
            ILogger<OrderBffController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var result = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders(PaginatedItemsRequest request)
        {
            var result = await _orderService.GetOrdersAsync(request.PageSize, request.PageIndex);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int?), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.CreateOrderAsync(
                request.CustomerId,
                (BikeStores.Models.Enums.OrderStatusEnum)request.OrderStatus,
                request.OrderDate,
                request.RequiredDate,
                request.ShippedDate,
                request.StoreId,
                request.OrderId,
                request.OrderItems);
            return Ok(result);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(CancelOrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            return Ok(new CancelOrderResponse() { IsCanceled = result });
        }
    }
}