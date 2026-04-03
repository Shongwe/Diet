using Microsoft.EntityFrameworkCore;
using Diet.Data;
using Diet.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. Register the DbContext so the BookingController can use the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Register the EmailService so it can be injected into your controllers
builder.Services.AddScoped<EmailService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Add this if you have images/css in wwwroot (crucial for Azure/IIS)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Note: MapStaticAssets is a .NET 9 feature. 
// If you are on .NET 8 or lower, use app.UseStaticFiles() instead.
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();