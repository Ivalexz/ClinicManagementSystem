using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class MedicalCard
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}