using Diet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Diet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var model = new HomeViewModel
        {
            // Populate the Stats collection
            Stats = new List<StatItem>
        {
            new StatItem("9+", "Years Experience", "bi-clock"),
            new StatItem("1.2k", "Happy Clients", "bi-people"),
            new StatItem("15+", "Medical Aids", "bi-shield-check"),
            new StatItem("4.9", "Average Rating", "bi-star")
        },
            // Populate the Services collection
            Services = new List<ServiceItem>
        {
            new ServiceItem("Diabetes Care", "Specialized meal planning for glucose control.", "bi-droplet", "bg-primary"),
            new ServiceItem("Weight Management", "Sustainable obesity and weight loss strategies.", "bi-activity", "bg-success")
        }
        };
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return View(model);
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
