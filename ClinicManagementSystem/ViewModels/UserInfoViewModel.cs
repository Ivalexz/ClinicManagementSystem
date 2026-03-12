using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.ViewModels
{
    public class UserInfoViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
