using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace END_Project.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Image { get; set; }
        //[Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsMale { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string Surname { get; set; }

        //[Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string FatherName { get; set; }

        //[Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string PhoneNumber { get; set; }
        public int Salary { get; set; }
        public DateTime DateofBirth { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa xananı doldurun")]

        [DataType(DataType.EmailAddress, ErrorMessage = "Zəhmət olmasa xananı doldurun")]
        public string Email { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }
        public bool IsDeactive { get; set; }
      

    }

}
