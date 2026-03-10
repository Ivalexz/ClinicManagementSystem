using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; }

        [Required]
        [StringLength(200)]
        public string EducationDocument { get; set; }
    }
}