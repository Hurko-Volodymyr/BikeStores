using BikeStores.Models;

namespace Order.Host.Models.Dtos
{
    public class OrderDto
    {
        public int CustomerId { get; set; }

        public Customer? Customer { get; internal set; }

        public byte OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int StoreId { get; set; }

        public Store? Store { get; internal set; }

        public int StaffId { get; set; }

        public Staff? Staff { get; internal set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
