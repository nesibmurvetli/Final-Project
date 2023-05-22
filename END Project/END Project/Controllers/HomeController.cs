using END_Project.DAL;
using END_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace END_Project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        //-----------------------------------------------//
        #region AppDbContext
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        #endregion
        //-----------------------------------------------//
        #region Index
        public async Task<IActionResult> Index()
        {
            Cash? cashes = await _db.Cashes.FirstOrDefaultAsync();
            return View(cashes);
        }
        #endregion
        //-----------------------------------------------//
    }
}