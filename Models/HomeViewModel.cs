namespace Diet.Models
{
    public class HomeViewModel
    {
        // SEO & Metadata
        public string MetaDescription { get; set; } = "Jeanett Mampane is a Registered Dietitian specializing in Diabetes and Obesity management with over 9 years of experience.";

        // Core Data
        public string SpecialistName { get; set; } = "Jeanett Mampane";
        public decimal ConsultationPrice { get; set; } = 650;

        // Collections
        public List<StatItem> Stats { get; set; } = new();
        public List<ServiceItem> Services { get; set; } = new();
        public List<string> SpecialistBadges { get; set; } = new()
    {
        "HPCSA Registered", "Diabetes Educator", "Medical Aid Accepted"
    };
    }

    public record StatItem(string Value, string Label, string Icon);
    public record ServiceItem(string Title, string Description, string Icon, string ColorClass);
}
