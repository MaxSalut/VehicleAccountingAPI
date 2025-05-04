using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleAccountingAPI.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Довжина має бути до 100символів")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Довжина має бути до 100символів")]
        public string Model { get; set; } = string.Empty;

            [Range(1900, 2025, ErrorMessage = "Рік повинен бути між 1900 і 2025")]
        public int Year { get; set; }

        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }

        public VehicleType? VehicleType { get; set; }

        public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
    }
}