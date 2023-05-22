using END_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace END_Project.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<MainMenu> MainMenus { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<ExtraMenu> ExtraMenus { get; set; }
        public DbSet<Fast> Fasts { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expenditure> Expenditures { get; set; }
        public DbSet<Cash> Cashes { get; set; }
        public DbSet<Wage> Wages { get; set; }

    }
}
