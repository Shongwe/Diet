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

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
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
                model.EndTime = model.StartTime.AddHours(1);
                model.IsConfirmed = false; // You can confirm later via email

                _context.Appointments.Add(model);
                await _context.SaveChangesAsync();

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
