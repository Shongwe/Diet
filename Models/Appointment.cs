using System.ComponentModel.DataAnnotations;

namespace Diet.Models
{
  
        public class Appointment
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string PhoneNumber { get; set; }

            [Required]
            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }

            public bool IsConfirmed { get; set; } = false;

            public string? Notes { get; set; }
        }
    }