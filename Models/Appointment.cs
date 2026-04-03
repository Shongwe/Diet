using System.ComponentModel.DataAnnotations;

namespace Diet.Models
{
        public class Appointment
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string PatientName { get; set; }

            [Required, EmailAddress]
            public string PatientEmail { get; set; }

            [Required]
            public DateTime StartTime { get; set; }

            [Required]
            public DateTime EndTime { get; set; }

            public string Description { get; set; }

            public bool IsConfirmed { get; set; } = false;
    }
}
