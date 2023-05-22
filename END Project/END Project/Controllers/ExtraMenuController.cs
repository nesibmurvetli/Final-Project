using END_Project.DAL;
using END_Project.Helpers;
using END_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace END_Project.Controllers
{
    public class ExtraMenuController : Controller
    {
        //-----------------------------------------------//
        #region sql
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ExtraMenuController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        //-----------------------------------------------//
        #region Index
        public async Task<IActionResult> Index()
        {
            List<ExtraMenu> ExtraMenus = await _db.ExtraMenus.Include(f => f.Fasts).ToListAsync();
            return View(ExtraMenus);
        }
        #endregion
        //-----------------------------------------------//
        #region CreateExtra
        public IActionResult CreateExtra()
        {
            return View();
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExtra(ExtraMenu extra)
        {
            if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            {
                return View();
            }
            bool isExist = await _db.ExtraMenus.AnyAsync(x => x.Name == extra.Name);   //database de var və yox olduğun yoxlamaq üçün
            if (isExist)
            {
                ModelState.AddModelError("Name", "Artıq yaratmısınız");
                return View();
            }
            await _db.ExtraMenus.AddAsync(extra);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region CreateFast
        public IActionResult CreateFast()
        {
            return View();
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFast(Fast fast, int extraId)
        {
            bool isExist = await _db.Fasts.AnyAsync(x => x.Name == fast.Name);   //database de var və yox olduğun yoxlamaq üçün
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu artıq mövcuddur");
                return View();
            }
            if (fast.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            if (!fast.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin");
                return View();
            }
            if (fast.Photo.Max2Mb())
            {
                ModelState.AddModelError("Photo", "Çəkilin ölçüsü maxsimal olaraq 2mb olmalıdır");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "assets", "projecphotos");
            fast.Image = await fast.Photo.SaveFileAsync(folder);
            fast.ExtraMenuId = extraId;
            await _db.Fasts.AddAsync(fast);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region UpdateExtraMenu
        public async Task<IActionResult> UpdateExtraMenu(int? extraId)
        {
            if (extraId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            ExtraMenu? dbExtraMenu = await _db.ExtraMenus.FirstOrDefaultAsync(x => x.Id == extraId);
            if (dbExtraMenu == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbExtraMenu);
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateExtraMenu(int? extraId, ExtraMenu extra)
        {
            if (extraId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            ExtraMenu? dbExtraMenu = await _db.ExtraMenus.FirstOrDefaultAsync(x => x.Id == extraId);
            if (dbExtraMenu == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            {
                return View(dbExtraMenu);
            }
            dbExtraMenu.Name = extra.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region UpdateFast
        public async Task<IActionResult> UpdateFast(int? fastId)
        {
            if (fastId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Fast? dbFasts = await _db.Fasts.FirstOrDefaultAsync(x => x.Id == fastId);
            if (dbFasts == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbFasts);
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFast(int? fastId, Fast fast)
        {
            if (fastId == null)  /*id si olmayanın yoxlanılmasınının qarşısının almaq üçün*/
            {
                return NotFound();
            }
            Fast? dbFasts = await _db.Fasts.FirstOrDefaultAsync(x => x.Id == fastId);

            if (dbFasts == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            {
                return View(dbFasts);
            }
            if (fast.Photo != null)
            {
                if (!fast.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin");
                    return View();

                }
                if (fast.Photo.Max2Mb())
                {
                    ModelState.AddModelError("Photo", "Çəkilin ölçüsü maxsimal olaraq 2mb olmalıdır");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "projecphotos");
                string path = Path.Combine(folder,path2: dbFasts.Image);  /*kohne şekili yenisi ile evez et*/
                if (System.IO.File.Exists(path))  /*databasede  de kohne sekil tapıldissa sil onu*/
                {
                    System.IO.File.Delete(path);
                }
                dbFasts.Image = await fast.Photo.SaveFileAsync(folder);
            }

            dbFasts.Name = fast.Name;
            dbFasts.Price = fast.Price;
            dbFasts.Ingradient = fast.Ingradient;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region AktivExtraMenu
        public async Task<IActionResult> AktivExtraMenu(int? extraId)    /*//deletin post metodu*/
        {
            if (extraId == null)
            {
                return NotFound();
            }
            ExtraMenu? dbExtraMenu = await _db.ExtraMenus.FirstOrDefaultAsync(x => x.Id == extraId);
            if (dbExtraMenu == null)
            {
                return BadRequest();
            }
            if (dbExtraMenu.IsDeactive)
            {
                dbExtraMenu.IsDeactive = false;
            }
            else
            {
                dbExtraMenu.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region AktivFast
        public async Task<IActionResult> AktivFast(int? fastId)    /*//deletin post metodu*/
        {
            if (fastId == null)
            {
                return NotFound();
            }
            Fast?  dbFast = await _db.Fasts.FirstOrDefaultAsync(x => x.Id == fastId);
            if (dbFast == null)
            {
                return BadRequest();
            }
            if (dbFast.IsDeactive)
            {
                dbFast.IsDeactive = false;
            }
            else
            {
                dbFast.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region DetailFast
        public async Task<IActionResult> DetailFast(int? fastId)
        {
            if (fastId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Fast? dbFast = await _db.Fasts.FirstOrDefaultAsync(x => x.Id == fastId);
            if (dbFast == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbFast);
        }
        #endregion
        //-----------------------------------------------//
    }
}
