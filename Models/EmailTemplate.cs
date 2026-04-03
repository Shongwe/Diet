namespace Diet.Models
{
    public class EmailTemplate
    {
       
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "PatientConfirmation"
        public string Subject { get; set; }
        public string Body { get; set; } // This will hold the HTML content
        
    }
}
