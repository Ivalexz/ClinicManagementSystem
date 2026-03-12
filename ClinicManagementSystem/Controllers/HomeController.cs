using System.Diagnostics;
using ClinicManagementSystem;
using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity!.IsAuthenticated)
            return View();

        if (User.IsInRole("SuperAdmin"))
        {
            var admins       = await _userManager.GetUsersInRoleAsync("Admin");
            var doctors      = await _userManager.GetUsersInRoleAsync("Doctor");
            var clients      = await _userManager.GetUsersInRoleAsync("User");
            var apptCount    = await _context.Appointments.CountAsync();

            ViewBag.AdminCount       = admins.Count;
            ViewBag.DoctorCount      = doctors.Count;
            ViewBag.ClientCount      = clients.Count;
            ViewBag.AppointmentCount = apptCount;
        }
        else if (User.IsInRole("Admin"))
        {
            var today        = DateTime.Today;
            var clients      = await _userManager.GetUsersInRoleAsync("User");
            var animalCount  = await _context.Animals.CountAsync();
            var apptCount    = await _context.Appointments.CountAsync();
            var todayCount   = await _context.Appointments
                                    .Where(a => a.DateTime.Date == today)
                                    .CountAsync();

            ViewBag.ClientCount          = clients.Count;
            ViewBag.AnimalCount          = animalCount;
            ViewBag.AppointmentCount     = apptCount;
            ViewBag.TodayAppointments    = todayCount;
        }
        else if (User.IsInRole("Doctor"))
        {
            var userId   = _userManager.GetUserId(User);
            var doctor   = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor != null)
            {
                var today     = DateTime.Today;
                var weekStart = today.AddDays(-(int)today.DayOfWeek + 1);
                var weekEnd   = weekStart.AddDays(7);

                var todayList = await _context.Appointments
                    .Include(a => a.Animal)
                    .Where(a => a.DoctorId == doctor.Id && a.DateTime.Date == today)
                    .OrderBy(a => a.DateTime)
                    .ToListAsync();

                var weekCount  = await _context.Appointments
                    .Where(a => a.DoctorId == doctor.Id
                             && a.DateTime >= weekStart
                             && a.DateTime < weekEnd)
                    .CountAsync();

                var totalCount = await _context.Appointments
                    .Where(a => a.DoctorId == doctor.Id)
                    .CountAsync();

                ViewBag.TodayList           = todayList;
                ViewBag.TodayAppointments   = todayList.Count;
                ViewBag.WeekAppointments    = weekCount;
                ViewBag.TotalAppointments   = totalCount;
            }
        }
        else // user
        {
            var userId  = _userManager.GetUserId(User);
            var animals = await _context.Animals
                .Where(a => a.ClientId == userId)
                .ToListAsync();

            ViewBag.Animals = animals;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
