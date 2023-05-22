using System.ComponentModel.DataAnnotations;

namespace END_Project.Models
{
    public class MainMenu
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string? Name { get; set; }
        public bool IsDeactive { get; set; }
        public List<Food>? Foods { get; set; }

    }
}
