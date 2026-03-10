using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Duration { get; set; }

        [Required]
        [StringLength(100)]
        public string Frequency { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}