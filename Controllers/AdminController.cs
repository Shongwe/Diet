using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diet.Data; // Ensure this matches your namespace
using Diet.Services; // Ensure this matches your EmailService namespace

namespace Diet.Controllers
{
    public class AdminController : Controller // Must inherit from Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        // Dependency Injection
        public AdminController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: /Admin/Index
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();
            return View(appointments); // Looks for Views/Admin/Index.cshtml
        }

        // POST: /Admin/ConfirmAppointment/5
        [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return Json(new { success = false, message = "Not found" });

            appointment.IsConfirmed = true;
            await _context.SaveChangesAsync(); // Updates SHONGWE\SQLEXPRESS01

            try
            {
                await _emailService.SendPatientThankYou(appointment.Email, appointment.FullName);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Confirmed, but email failed: " + ex.Message });
            }

            return Json(new { success = true, message = "Appointment confirmed and email sent!" });
        }
    }
}
