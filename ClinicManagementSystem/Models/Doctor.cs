using System.ComponentModel.DataAnnotations;
using ClinicManagementSystem.Models;

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

    public int ExperienceYears { get; set; }

    public string OfficeNumber { get; set; }

    public string Phone { get; set; }

    public string Description { get; set; }
}