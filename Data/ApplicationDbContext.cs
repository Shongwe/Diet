using Microsoft.EntityFrameworkCore;
using Diet.Models;

namespace Diet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmailTemplate>  EmailTemplates { get; set; }
    }
}