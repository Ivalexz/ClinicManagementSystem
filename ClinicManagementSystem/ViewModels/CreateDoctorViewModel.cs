using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models.ViewModels
{
    public class CreateDoctorViewModel
    {

        public ApplicationUser User { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public Doctor Doctor { get; set; }
    }
}