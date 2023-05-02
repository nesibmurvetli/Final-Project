using END_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace END_Project.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<MainMenu> MainMenus { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<ExtraMenu> ExtraMenus { get; set; }
        public DbSet<Fast> Fasts { get; set; }
    }
}
