using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicManagementSystem.ViewModels;

namespace ClinicManagementSystem.Controllers;

[Authorize]
public class InfoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public InfoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [Authorize(Roles = "User")]
    public async Task<IActionResult> MyAnimals()
    {
        var userId = _userManager.GetUserId(User);
        
        var animals = await _context.Animals
            .Include(a => a.MedicalCard)
                .ThenInclude(m => m.Appointments)
                    .ThenInclude(ap => ap.Doctor)
                        .ThenInclude(d => d.User)
            .Include(a => a.MedicalCard)
                .ThenInclude(m => m.Appointments)
                    .ThenInclude(ap => ap.Prescriptions)
            .Where(a => a.ClientId == userId)
            .ToListAsync();

        return View(animals);
    }
    
    [Authorize(Roles = "User")]
    public async Task<IActionResult> MyAnimalMedicalCard(int animalId)
    {
        var userId = _userManager.GetUserId(User);
        
        var animal = await _context.Animals
            .Include(a => a.MedicalCard)
                .ThenInclude(m => m.Appointments)
                    .ThenInclude(ap => ap.Doctor)
                        .ThenInclude(d => d.User)
            .Include(a => a.MedicalCard)
                .ThenInclude(m => m.Appointments)
                    .ThenInclude(ap => ap.Prescriptions)
            .FirstOrDefaultAsync(a => a.Id == animalId && a.ClientId == userId);

        if (animal == null)
        {
            return NotFound();
        }

        return View("MedicalCardDetails", animal);
    }
    
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> AllUsers()
    {
        var users = await _userManager.GetUsersInRoleAsync("User");
        
        var userList = new List<UserInfoViewModel>();
        
        foreach (var user in users)
        {
            var animals = await _context.Animals
                .Include(a => a.MedicalCard)
                .Where(a => a.ClientId == user.Id)
                .ToListAsync();

            userList.Add(new UserInfoViewModel
            {
                User = user,
                Animals = animals
            });
        }

        return View(userList);
    }
    
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> UserAnimals(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var animals = await _context.Animals
            .Include(a => a.MedicalCard)
                .ThenInclude(m => m.Appointments)
                    .ThenInclude(ap => ap.Doctor)
                        .ThenInclude(d => d.User)
            .Include(a => a.MedicalCard)
                .ThenInclude(m => m.Appointments)
                    .ThenInclude(ap => ap.Prescriptions)
            .Where(a => a.ClientId == userId)
            .ToListAsync();

        ViewBag.User = user;
        return View(animals);
    }
    
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> FullInfo()
    {
        var viewModel = new FullInfoViewModel();
        
        var users = await _userManager.GetUsersInRoleAsync("User");
        viewModel.Users = users.ToList();

        var admins = await _userManager.GetUsersInRoleAsync("Admin");
        viewModel.Admins = admins.ToList();

        var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
        var doctorIds = doctors.Select(d => d.Id).ToList();
        
        viewModel.Doctors = await _context.Doctors
            .Include(d => d.User)
            .Where(d => doctorIds.Contains(d.UserId))
            .ToListAsync();

        viewModel.Animals = await _context.Animals
            .Include(a => a.Client)
            .Include(a => a.MedicalCard)
            .ToListAsync();

        return View(viewModel);
    }
}