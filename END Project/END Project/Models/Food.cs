using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace END_Project.Models
{
    public class Food
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string? Name { get; set; } 
        public string? Image { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string? Ingradient { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public int Price { get; set; }
        public bool IsDeactive { get; set; }
        public MainMenu? MainMenus { get; set; }
        public int MainMenuId { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
   
    }
}
