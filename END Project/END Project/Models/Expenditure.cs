
using System.ComponentModel.DataAnnotations;

namespace END_Project.Models
{
    public class Expenditure
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilmez")]
        public string? For { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Bu xana boş ola bilmez")]
        public float Money { get; set; }
        public DateTime StartTime { get; set; }
        public AppUser? AppUser { get; set; }
        public string? AppUserId { get; set; }
    }
}
