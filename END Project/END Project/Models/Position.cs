
namespace END_Project.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string? PosName { get; set; }
        public string? Responsibilities { get; set; }
        public bool IsDeactive { get; set; }
        public List<Employee>? Employees { get; set; }

    }
}
