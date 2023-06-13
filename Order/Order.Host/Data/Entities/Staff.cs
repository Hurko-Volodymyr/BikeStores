using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStores.Models
{
    [Table("staffs", Schema = "sales")]
    public class Staff
    {
        [Key]
        [Column("staff_id")]
        public int StaffId { get; set; }

        [Required]
        [Column("first_name")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty!;

        [Required]
        [Column("last_name")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty!;

        [Required]
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty!;

        [Column("phone")]
        [StringLength(25)]
        public string? Phone { get; set; }

        [Required]
        [Column("active")]
        public byte Active { get; set; }

        [Required]
        [Column("store_id")]
        [ForeignKey("Store")]
        public int StoreId { get; set; }

        [Column("manager_id")]
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; } = default!;

        [ForeignKey("ManagerId")]
        public Staff Manager { get; set; } = default!;
    }
}
