using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.ViewModels
{
    public class FullInfoViewModel
    {
        public List<ApplicationUser> Users { get; set; } = new();
        public List<ApplicationUser> Admins { get; set; } = new();
        public List<Doctor> Doctors { get; set; } = new();
        public List<Animal> Animals { get; set; } = new();
    }
}