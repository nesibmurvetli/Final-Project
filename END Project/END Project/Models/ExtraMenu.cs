using System.ComponentModel.DataAnnotations;

namespace END_Project.Models
{
    public class ExtraMenu
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string? Name { get; set; }
        public bool IsDeactive { get; set; }
        public List<Fast>? Fasts { get; set; }
    }
}
