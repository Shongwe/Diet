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
                // Default duration logic
                if (model.EndTime == DateTime.MinValue || model.EndTime <= model.StartTime)
                {
                    model.EndTime = model.StartTime.AddMinutes(30);
                }
                model.IsConfirmed = false;

                _context.Appointments.Add(model);
                await _context.SaveChangesAsync();

                try
                {
                    await _emailService.SendBookingNotification(model.FullName, model.Email, model.StartTime);
                    await _emailService.SendPatientThankYou(model.Email, model.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Email failed: " + ex.Message);
                }

                // FIX: Instead of RedirectToAction, return the URL to the frontend
                // This allows your JavaScript 'fetch' to perform the actual redirection
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Success", "Booking")
                });
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet]
        public JsonResult GetBookedSlots()
        {
            var appointments = _context.Appointments
                .Select(a => new {
                    title = "Booked",
                    start = a.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = a.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    display = "background",
                    color = "#d3d3d3"
                })
                .ToList();

            return Json(appointments);
        }

        // GET: /Booking/Success
        public IActionResult Success()
        {
            // This returns the view modeled after the Gabriel Optical layout
            return View();
        }
    }
}