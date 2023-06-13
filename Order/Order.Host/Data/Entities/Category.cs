using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models
{
    [Table("categories", Schema = "production")]
    public class Category
    {
        [Key]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("category_name")]
        public string CategoryName { get; set; } = default!;
    }
}