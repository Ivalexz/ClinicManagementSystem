using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Controllers;

[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(ApplicationUser model, string password)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _userManager.CreateAsync(model, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(model, "User");
            return RedirectToAction(nameof(Users));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(ApplicationUser model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null) return NotFound();

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.UserName = model.Email;

        await _userManager.UpdateAsync(user);

        return RedirectToAction(nameof(Users));
    }

    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        await _userManager.DeleteAsync(user);
        return RedirectToAction(nameof(Users));
    }
    // Тварини 
    public async Task<IActionResult> Animals()
    {
        var animals = await _context.Animals
            .Include(a => a.Client)
            .Include(a => a.MedicalCard)
            .ToListAsync();

        return View(animals);
    }

    public IActionResult CreateAnimal()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnimal(Animal model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var card = new MedicalCard
        {
            CreateDate = DateTime.Now
        };

        var animal = new Animal
        {
            Name = model.Name,
            Species = model.Species,
            Breed = model.Breed,
            Gender = model.Gender,
            DateOfBirth = model.DateOfBirth,
            ClientId = model.ClientId,
            MedicalCard = card
        };

        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Animals));
    }

    public async Task<IActionResult> DeleteAnimal(int id)
    {
        var animal = await _context.Animals.FindAsync(id);
        if (animal == null) return NotFound();

        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Animals));
    }

    public async Task<IActionResult> MedicalCard(int id)
    {
        var card = await _context.MedicalCards
            .Include(c => c.Appointments)
            .ThenInclude(a => a.Doctor)
            .Include(c => c.Appointments)
            .ThenInclude(a => a.Prescriptions)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (card == null) return NotFound();

        return View(card);
    }
}