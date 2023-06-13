using System.Security.Claims;
using Catalog.Host.Models.Requests.Items;
using Catalog.Host.Models.Response.Items;
using IdentityModel;
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
            var userId = User.FindFirstValue(JwtClaimTypes.Subject);

            // if (userId!.Equals(result?.CustomerId.ToString()))
            // {
            //    throw new UnauthorizedAccessException("User is not authorized to see the order.");
            // }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders(PaginatedItemsRequest request)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Subject);
            var result = await _orderService.GetOrdersAsync(request.PageSize, request.PageIndex);

            // if (userId!.Equals(result?.Data?.FirstOrDefault()?.CustomerId.ToString()))
            // {
            //    throw new UnauthorizedAccessException("User is not authorized to see the order.");
            // }
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

            // var userId = User.FindFirstValue(JwtClaimTypes.Subject);
            // if (userId!.Equals(request.CustomerId.ToString()))
            // {
            //    throw new UnauthorizedAccessException("User is not authorized to add the order.");
            // }
            var result = await _orderService.CreateOrderAsync(
                request.CustomerId,
                (BikeStores.Models.Enums.OrderStatusEnum)request.OrderStatus,
                request.OrderDate,
                request.RequiredDate,
                request.ShippedDate,
                request.StoreId,
                request.StaffId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CancelOrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Subject);
            var testId = await _orderService.GetOrderByIdAsync(id);
            var result = await _orderService.CancelOrderAsync(id);

            // if (userId!.Equals(testId?.CustomerId.ToString()))
            // {
            //    throw new UnauthorizedAccessException("User is not authorized to see the order.");
            // }
            return Ok(new CancelOrderResponse() { IsCanceled = result });
        }
    }
}
