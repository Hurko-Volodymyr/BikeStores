using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models
{
    [Table("order_items", Schema = "sales")]
    public class OrderItem
    {
        [Column("order_id")]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Required]
        [Column("product_id")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("list_price", TypeName = "decimal(10, 2)")]
        public decimal ListPrice { get; set; }

        [Required]
        [Column("discount", TypeName = "decimal(4, 2)")]
        public decimal Discount { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
