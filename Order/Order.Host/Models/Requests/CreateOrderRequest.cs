using BikeStores.Models;

namespace Order.Host.Models.Requests
{
    public class CreateOrderRequest
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; internal set; } = null!;
        public byte OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; internal set; } = null!;
        public int StaffId { get; set; }
        public Staff Staff { get; internal set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = null!;
    }
}
