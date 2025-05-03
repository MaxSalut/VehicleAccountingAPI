using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleAccountingAPI.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int Year { get; set; }

        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }

        public VehicleType? VehicleType { get; set; }

        public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
    }
}