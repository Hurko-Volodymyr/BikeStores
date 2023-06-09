using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models
{
    [Table("stores", Schema = "sales")]
    public class Store
    {
        [Key]
        [Column("store_id")]
        public int StoreId { get; set; }

        [Required]
        [Column("store_name")]
        [StringLength(255)]
        public string StoreName { get; set; }

        [Column("phone")]
        [StringLength(25)]
        public string Phone { get; set; }

        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; }

        [Column("street")]
        [StringLength(255)]
        public string Street { get; set; }

        [Column("city")]
        [StringLength(255)]
        public string City { get; set; }

        [Column("state")]
        [StringLength(10)]
        public string State { get; set; }

        [Column("zip_code")]
        [StringLength(5)]
        public string ZipCode { get; set; }
    }
}
