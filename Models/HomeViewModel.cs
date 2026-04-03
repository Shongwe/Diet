namespace Diet.Models
{
public class HomeViewModel
    {
        public decimal ConsultationPrice { get; set; } = 650;
        public string SpecialistName { get; set; } = "Jeanett Mampane";
        public List<string> TeamMembers { get; set; } = new() { "Laura Manyike", "Lethabo Meso", "Beverly Gibbs" };
        public List<StatItem> Stats { get; set; } = new()
    {
        new("9+", "Years Experience", "bi-clock-history"),
        new("1,000+", "Patients Helped", "bi-people"),
        new("15+", "Certifications", "bi-award"),
        new("4", "Dietitians Team", "bi-hospital")
    };
    }

    public record StatItem(string Value, string Label, string Icon);
}
