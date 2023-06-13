using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models
{
    [Table("customers", Schema = "sales")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Required]
        [Column("first_name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Column("last_name")]
        public string LastName { get; set; } = null!;

        [Column("phone")]
        public string? Phone { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("street")]
        public string Street { get; set; } = null!;

        [Column("city")]
        public string City { get; set; } = null!;

        [Column("state")]
        public string State { get; set; } = null!;

        [Column("zip_code")]
        public string ZipCode { get; set; } = null!;
    }
}