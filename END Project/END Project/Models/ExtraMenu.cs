using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace END_Project.Models
{
    public class ExtraMenu
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string Name { get; set; }
        public bool IsDeactive { get; set; }
        public List<Fast> Fasts { get; set; }
    }
}
