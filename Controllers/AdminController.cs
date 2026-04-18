using Diet.Data;
using Diet.Models;
using Diet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Diet.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public AdminController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Specifically points to your file in Views/User/Index.cshtml
            return View("~/Views/User/Index.cshtml");
        }

        // POST: /Admin/Login
     

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Replace with your actual credential check
            if (model.Email == "admin@njdietitians.com" && model.Password == "Admin123!")
            {
                var claims = new List<Claim>
        {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, "Admin")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Dashboard", "Admin");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // This clears the authentication cookie from the browser's shared storage
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear session if you are using it
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
        // GET: /Admin/Dashboard
        // Move your appointment list logic here
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var appointments = await _context.Appointments
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            // This will look for Views/Admin/Dashboard.cshtml
            return View(appointments);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound(new { success = false, message = "Booking not found." });
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Booking deleted successfully." });
        }
        // POST: /Admin/ConfirmAppointment/5
        [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return Json(new { success = false, message = "Not found" });

            appointment.IsConfirmed = true;
            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendPatientThankYou(appointment.Email, appointment.FullName);
                return Json(new { success = true, message = "Appointment confirmed and email sent!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Confirmed, but email failed: " + ex.Message });
            }
        }
    }
}