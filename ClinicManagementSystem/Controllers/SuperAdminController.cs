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
        return View(new CreateDoctorViewModel
        {
            User = new ApplicationUser(),
            Doctor = new Doctor()
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor(CreateDoctorViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        vm.User.UserName = vm.User.Email;

        var result = await _userManager.CreateAsync(vm.User, vm.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(vm.User, "Doctor");

            vm.Doctor.UserId = vm.User.Id;

            _context.Doctors.Add(vm.Doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Doctors));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

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

        await _userManager.UpdateAsync(user);

        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        await _userManager.DeleteAsync(user);

        return RedirectToAction(nameof(Index));
    }

    
}