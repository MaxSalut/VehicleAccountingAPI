using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleAccountingAPI.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Vehicle? Vehicle { get; set; }
        public Driver? Driver { get; set; }
    }
}