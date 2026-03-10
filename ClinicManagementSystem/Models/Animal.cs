using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class Animal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Species { get; set; }

        [StringLength(50)]
        public string Breed { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; }

        public int MedicalCardId { get; set; }
        public MedicalCard MedicalCard { get; set; }
    }
}