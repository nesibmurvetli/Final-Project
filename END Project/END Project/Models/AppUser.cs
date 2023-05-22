using Microsoft.AspNetCore.Identity;

namespace END_Project.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public bool IsDeactive { get; set; }
        public bool IsMale { get; set; }
        List<Income> Incomes { get; set; }
        List<Expenditure> Expenditures { get; set; }

    }
}
