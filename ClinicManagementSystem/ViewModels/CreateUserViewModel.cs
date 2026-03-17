using System.ComponentModel.DataAnnotations;
namespace ClinicManagementSystem.ViewModels;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Поле обов'язкове")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Введіть ім'я")]
    [Display(Name = "Ім'я")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Введіть прізвище")]
    [Display(Name = "Прізвище")]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "{0} має бути від {2} до {1} символів.", 
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Підтвердження пароля")]
    [Compare("Password", ErrorMessage = "Паролі не збігаються")]
    public string ConfirmPassword { get; set; }
}