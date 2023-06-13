using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models
{
    [Table("products", Schema = "production")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("product_name")]
        public string ProductName { get; set; } = default!;

        [Required]
        [Column("brand_id")]
        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        [Required]
        [Column("category_id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        [Column("model_year")]
        public short ModelYear { get; set; }

        [Required]
        [Column("list_price", TypeName = "decimal(10, 2)")]
        public decimal ListPrice { get; set; }

        // public Brand Brand { get; set; }
        public Category Category { get; set; } = default!;
    }
}
