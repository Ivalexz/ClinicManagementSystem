using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(500)]
        public string Complaint { get; set; }

        [StringLength(500)]
        public string Diagnosis { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        [Range(0, 1000000)]
        public decimal Price { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}