using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models.ViewModels
{
    public class CreateDoctorViewModel
    {

        [Required(ErrorMessage = "Ім'я обов'язкове")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище обов'язкове")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [MinLength(6, ErrorMessage = "Пароль повинен містити мінімум 6 символів")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Спеціалізація обов'язкова")]
        public string Specialization { get; set; }

        public int ExperienceYears { get; set; }

        public string OfficeNumber { get; set; }

        public string Phone { get; set; }

        public string EducationDocument { get; set; }

        public string Description { get; set; }
    }
}