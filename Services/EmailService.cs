using Diet.Data; // Ensure this points to your ApplicationDbContext
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace Diet.Services
{
    public class EmailService
    {
        private readonly ApplicationDbContext _context;

        // Inject the context so SendPatientThankYou can access your database templates
        public EmailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendBookingNotification(string patientName, string patientEmail, DateTime startTime)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("NJ Seroka Booking", "info@njserokadietitians.co.za"));
            message.To.Add(new MailboxAddress("Admin", "admin@njserokadietitians.co.za"));
            message.Subject = "New Consultation Request: " + patientName;

            message.Body = new TextPart("html")
            {
                Text = $@"
                <div style='font-family: Arial, sans-serif; border: 1px solid #2ECC71; padding: 20px; border-radius: 10px;'>
                    <h2 style='color: #2ECC71;'>New Booking Received</h2>
                    <p><strong>Patient Name:</strong> {patientName}</p>
                    <p><strong>Patient Email:</strong> {patientEmail}</p>
                    <p><strong>Requested Date/Time:</strong> {startTime:f}</p>
                    <hr>
                    <p>Log in to your dashboard to confirm this appointment.</p>
                </div>"
            };

            await SendEmailAsync(message);
        }

        public async Task SendPatientThankYou(string patientEmail, string patientName)
        {
            // 1. Fetch the template from the DB
            var template = await _context.EmailTemplates
                .FirstOrDefaultAsync(t => t.Name == "PatientConfirmation");

            if (template == null) return; // Or handle with a default message

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("NJ Seroka Dietitians", "info@njserokadietitians.co.za"));
            message.To.Add(new MailboxAddress(patientName, patientEmail));
            message.Subject = template.Subject ?? "Consultation Confirmed!";

            // 2. Personalize the body
            string personalizedBody = template.Body.Replace("{Name}", patientName);
            message.Body = new TextPart("html") { Text = personalizedBody };

            await SendEmailAsync(message);
        }

        // Helper method to keep SMTP logic in one place
        private async Task SendEmailAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                // Note: On Azure, port 25 is blocked. Port 587 with StartTls is the way to go.
                await client.ConnectAsync("mail.njserokadietitians.co.za", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("info@njserokadietitians.co.za", "YourPassword");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}