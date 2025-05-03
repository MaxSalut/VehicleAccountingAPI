using System.ComponentModel.DataAnnotations;

namespace VehicleAccountingAPI.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string LicenseNumber { get; set; } = string.Empty;

        public List<Assignment> Assignments { get; set; } = new();
    }
}