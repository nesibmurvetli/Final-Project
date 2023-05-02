using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace END_Project.Models
{
    public class Fast
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string Ingradient { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public int Price { get; set; }
        public bool IsDeactive { get; set; }
        public ExtraMenu ExtraMenus { get; set; }
        public int ExtraMenuId { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
