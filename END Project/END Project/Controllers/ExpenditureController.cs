using END_Project.DAL;
using END_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace END_Project.Controllers
{
    public class ExpenditureController : Controller
    {
        //-----------------------------------------------//
        #region AppDbContext
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public ExpenditureController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        #endregion
        //-----------------------------------------------//
        #region Index
        public IActionResult Index()
        {
            List<Expenditure> expenditures = _db.Expenditures.Include(x => x.AppUser).ToList();
            return View(expenditures);
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
        public async Task<IActionResult> Create(Expenditure expenditure)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Cash? cash = await _db.Cashes.FirstOrDefaultAsync();
            cash.LastModifiedBy = user.Name;
            if (expenditure.Money > cash.Balance)
            {
                ModelState.AddModelError("Money", "Balansda kifayet qeder Pul yoxdur.");
                return View();
            }
            else
            {
                cash.Balance -= expenditure.Money;
            }
            cash.LastMotifiedMoney = expenditure.Money - expenditure.Money - expenditure.Money;
            cash.LastModified = expenditure.For;
            cash.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            expenditure.StartTime = DateTime.UtcNow.AddHours(4);
            AppUser? appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            expenditure.AppUserId = appUser.Id;
            await _db.Expenditures.AddAsync(expenditure);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
    }
}
