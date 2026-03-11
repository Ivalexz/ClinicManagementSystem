using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Animal)
                .Include(a => a.Doctor)
                .Include(a => a.Prescriptions)
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Animal)
                .Include(a => a.Doctor)
                .Include(a => a.Prescriptions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Prescriptions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
                return NotFound();

            appointment.Diagnosis = model.Diagnosis;
            appointment.Price = model.Price;
            appointment.Notes = model.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = appointment.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription(int appointmentId, Prescription prescription)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                return NotFound();

            prescription.AppointmentId = appointmentId;

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = appointmentId });
        }

        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);

            if (prescription == null)
                return NotFound();

            int appointmentId = prescription.AppointmentId;

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = appointmentId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}