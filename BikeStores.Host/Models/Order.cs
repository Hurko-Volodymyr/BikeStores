using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BikeStores.Models
{
    [Table("orders", Schema = "sales")]
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        [ForeignKey("sales.customers")]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        public Customer Customer { get; internal set; }

        [Required]
        [Column("order_status")]
        public byte OrderStatus { get; set; }

        [Required]
        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column("required_date")]
        public DateTime RequiredDate { get; set; }

        [Column("shipped_date")]
        public DateTime? ShippedDate { get; set; }

        [ForeignKey("sales.stores")]
        [Column("store_id")]
        public int StoreId { get; set; }

        public Store Store { get; internal set; }   

        [ForeignKey("sales.staffs")]
        [Column("staff_id")]
        public int StaffId { get; set; }

        public Staff Staff { get; internal set; }

         public List<OrderItem> OrderItems { get; set; }

    }
}