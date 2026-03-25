using ClinicManagementSystem.Models;
using ClinicManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ClinicManagementSystem.Controllers;

public class SuperAdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public SuperAdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Admins()
    {
        var admins = await _userManager.GetUsersInRoleAsync("Admin");
        return View(admins);
    }

    public async Task<IActionResult> Doctors()
    {
        var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
        return View(doctors);
    }
    
    public IActionResult CreateAdmin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdmin(ApplicationUser model, string password)
    {
        if (!ModelState.IsValid)
            return View(model);

        //валідація пароля
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            ModelState.AddModelError("", "Пароль повинен бути мінімум 6 символів");
            return View(model);
        }
        
        model.UserName = model.Email;
        var result = await _userManager.CreateAsync(model, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(model, "Admin");
            return RedirectToAction(nameof(Admins));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    public IActionResult CreateDoctor()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDoctor(CreateDoctorViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);
        

        var user = new ApplicationUser
        {
            UserName = vm.Email,
            Email = vm.Email,
            FirstName = vm.FirstName,
            LastName = vm.LastName
        };

        var result = await _userManager.CreateAsync(user, vm.Password);
        
        if (result.Succeeded)
        {
            var addRoleResult = await _userManager.AddToRoleAsync(user, "Doctor");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(vm);
            }
            
            var doctor = new Doctor
            {
                UserId = user.Id,
                Specialization = vm.Specialization,
                ExperienceYears = vm.ExperienceYears,
                OfficeNumber = vm.OfficeNumber,
                Phone = vm.Phone,
                EducationDocument = vm.EducationDocument,
                Description = vm.Description
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Doctors));
        }

        return View(vm);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null) return NotFound();

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.UserName = model.Email;

        var result = await _userManager.UpdateAsync(user);
 
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.DeleteAsync(user);
 
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        return RedirectToAction(nameof(Index));
    }

    
}