using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
