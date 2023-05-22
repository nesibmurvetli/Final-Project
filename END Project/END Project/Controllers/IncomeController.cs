using END_Project.DAL;
using END_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace END_Project.Controllers
{
    public class IncomeController : Controller
    {
        //-----------------------------------------------//
        #region AppDbContext
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public IncomeController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        #endregion
        //-----------------------------------------------//
        #region Index
        public IActionResult Index()
        {
            List<Income> incomes = _db.Incomes.Include(x => x.AppUser).ToList();
            return View(incomes);
        }
        #endregion
        //-----------------------------------------------//
        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Income income)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Cash? cash = await _db.Cashes.FirstOrDefaultAsync();
            cash.LastModifiedBy = user.Name;
            cash.Balance += income.Money;
            cash.LastMotifiedMoney = income.Money;
            cash.LastModified = income.For;
            cash.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            income.StartTime = DateTime.UtcNow.AddHours(4);
            AppUser? appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            income.AppUserId = appUser.Id;
            await _db.Incomes.AddAsync(income);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//


    }
}
