using END_Project.DAL;
using END_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace END_Project.Controllers
{
    public class WagesController : Controller
    {
        //-----------------------------------------------//
        #region AppDbContext
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public WagesController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        #endregion
        //-----------------------------------------------//
        #region Index
        public async Task<IActionResult> Index()
        {
            List<Wage> wages = await _db.Wages.Include(x => x.Employee).Include(x => x.AppUser).ToListAsync();
            return View(wages);
        }
        #endregion
        //-----------------------------------------------//
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = await _db.Employees.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Wage wage, int empId)
        {
            ViewBag.Employees = await _db.Employees.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            Employee? employees = await _db.Employees.FirstOrDefaultAsync(x => x.Id == empId);
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Cash? cash = await _db.Cashes.FirstOrDefaultAsync();
            cash.LastModifiedBy = user.Name;
            cash.Balance -= employees.Salary;
            cash.LastMotifiedMoney = employees.Salary - (float)employees.Salary - employees.Salary;
            if (wage.Money > cash.Balance)
            {
                ModelState.AddModelError("Employee", "Şirketin balansında kifayət qədər pul yoxdur.");
                return View();
            }
            cash.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            if (employees.IsMale)
            {
                cash.LastModified = $"{employees.Name} {employees.Surname} {employees.FatherName} oğluna maaş ödənildi";
            }
            else
            {
                cash.LastModified = $"{employees.Name} {employees.Surname} {employees.FatherName} qızına maaş ödənildi";
            }
            wage.EmployeeId = empId;
            wage.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            AppUser? appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            wage.AppUserId = appUser.Id;
            wage.Money = (float)employees.Salary;
            await _db.Wages.AddAsync(wage);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
    }
}
