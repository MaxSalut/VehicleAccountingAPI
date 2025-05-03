using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleAccountingAPI.Models
{
    public class MaintenanceRecord
    {
        [Key]
        public int MaintenanceRecordId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 1000000)]
        public decimal Cost { get; set; }

        public Vehicle? Vehicle { get; set; }
    }
}