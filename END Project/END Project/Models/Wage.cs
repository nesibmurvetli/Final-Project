
namespace END_Project.Models
{
    public class Wage
    {
        public int Id { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public Employee? Employee { get; set; }
        public int EmployeeId { get; set; }
        public float Money { get; set; }
        public AppUser? AppUser { get; set; }
        public string? AppUserId { get; set; }
    }
}
