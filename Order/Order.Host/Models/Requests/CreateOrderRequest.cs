using BikeStores.Models;
using BikeStores.Models.Enums;

namespace Order.Host.Models.Requests
{
    public class CreateOrderRequest
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public byte OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        [Required]
        public int StoreId { get; set; }

        [Required]
        public int StaffId { get; set; }
    }
}
