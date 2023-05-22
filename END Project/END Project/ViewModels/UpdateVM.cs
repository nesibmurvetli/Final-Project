
using System.ComponentModel.DataAnnotations;

namespace END_Project.ViewModels
{
    public class UpdateVM
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? Username { get; set; }
        public string? Role { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public bool IsMale { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
