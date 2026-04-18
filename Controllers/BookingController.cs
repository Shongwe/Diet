using Diet.Data;
using Diet.Models;
using Diet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diet.Controllers
{
    public class BookingController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;


        public BookingController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment model)
        {
            if (ModelState.IsValid)
            {
                // For a dietitian, appointments are usually 1 hour
                if (model.EndTime == DateTime.MinValue || model.EndTime <= model.StartTime)
                {
                    model.EndTime = model.StartTime.AddMinutes(30);
                }
                model.IsConfirmed = false; // You can confirm later via email

                _context.Appointments.Add(model);
                await _context.SaveChangesAsync();

                // Send Emails
                try
                {
                    // 1. Notify Admin
                    await _emailService.SendBookingNotification(model.FullName, model.Email, model.StartTime);

                    // 2. Thank the Patient using your DB template
                    await _emailService.SendPatientThankYou(model.Email, model.FullName);
                }
                catch (Exception ex)
                {
                    // Log email failure but don't stop the booking process
                    Console.WriteLine("Email failed: " + ex.Message);
                }

                return Json(new { success = true, message = "Appointment requested successfully!" });
            }
            return Json(new { success = false, message = "Invalid data provided." });
        }
        [HttpGet]
        public JsonResult GetBookedSlots()
        {

            // Fetch all future appointments to grey out the calendar
            var appointments = _context.Appointments
                .Select(a => new {
                    title = "Booked",
                    start = a.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = a.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    display = "background",
                    color = "#d3d3d3" // Grey color for unavailable slots
                })
                .ToList();

            return Json(appointments);
        }
    }
}
