using END_Project.DAL;
using END_Project.Helpers;
using END_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace END_Project.Controllers
{
    public class MainMenuController : Controller
    {
        //-----------------------------------------------//
        #region sql
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public MainMenuController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        //-----------------------------------------------//
        #region İndex
        public async Task<IActionResult> Index()
        {
            List<MainMenu> MainMenu = await _db.MainMenus.Include(f=>f.Foods).ToListAsync();
            return View(MainMenu);
        }
        #endregion
        //-----------------------------------------------//
        #region CreateMenu
        public IActionResult CreateMenu()
        {
            return View();
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMenu(MainMenu menu)
        {
            bool isExist = await _db.MainMenus.AnyAsync(x => x.Name == menu.Name);   //database de var və yox olduğun yoxlamaq üçün
            if (isExist)
            {
                ModelState.AddModelError("Name", "Artıq yaratmısınız");
                return View();
            }
            await _db.MainMenus.AddAsync(menu); 
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region CreateFood
        public IActionResult CreateFood()
        { 
            return View();
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFood(Food food,int menuId)
        {
            bool isExist = await _db.Foods.AnyAsync(x => x.Name == food.Name);   //database de var və yox olduğun yoxlamaq üçün
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu artıq mövcuddur");
                return View();
            }
            if (food.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            if (!food.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin");
                return View();
            }
            if (food.Photo.Max2Mb())
            {
                ModelState.AddModelError("Photo", "Şəkilin ölçüsü maxsimal olaraq 2mb olmalıdır");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "assets","projecphotos");  
            food.Image = await food.Photo.SaveFileAsync(folder);
            food.MainMenuId = menuId;
            await _db.Foods.AddAsync(food);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region UpdateMainMenu
        public async Task<IActionResult> UpdateMainMenu(int? menuId)
        {
            if (menuId == null)  /*id si olmayanın yoxlanılmasınının qarşısını almaq üçün*/
            {
                return NotFound();
            }
            MainMenu? dbMainMenu = await _db.MainMenus.FirstOrDefaultAsync(x => x.Id == menuId);
            if (dbMainMenu == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbMainMenu);
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMainMenu(int? menuId, MainMenu menu)
        {
            if (menuId == null)  /*id si olmayanın yoxlanılmasınının qarşısını almaq üçün*/
            {
                return NotFound();
            }
            MainMenu? dbMainMenu = await _db.MainMenus.FirstOrDefaultAsync(x => x.Id == menuId);
            if (dbMainMenu == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            {
                return View(dbMainMenu);
            }
            dbMainMenu.Name = menu.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region UpdateFood
        public async Task<IActionResult> UpdateFood(int? foodId)
        {
            if (foodId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Food? dbFoods = await _db.Foods.FirstOrDefaultAsync(x => x.Id == foodId);
            if (dbFoods == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbFoods);
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFood(int? foodId, Food food/*,Food dbFoods*/)
        {
            if (foodId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Food? dbFoods = await _db.Foods.FirstOrDefaultAsync(x => x.Id == foodId);
         
            if (dbFoods == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            if (food.Photo != null)
            {
                if (!food.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin");
                    return View();

                }
                if (food.Photo.Max2Mb())
                {
                    ModelState.AddModelError("Photo", "Şəkilin ölçüsü maxsimal olaraq 2mb olmalıdır");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "projecphotos");
                string path = Path.Combine(folder, path2: dbFoods.Image);  /*kohne şekili yenisi ile evez et*/
                if (System.IO.File.Exists(path))  /*databasede  de kohne sekil tapıldissa sil onu*/
                {
                    System.IO.File.Delete(path);
                }
                dbFoods.Image = await food.Photo.SaveFileAsync(folder);
            }
          
            dbFoods.Name = food.Name;
           
            dbFoods.Price = food.Price;
            dbFoods.Ingradient = food.Ingradient;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region AktivMainMenu
        public async Task<IActionResult> AktivMainMenu(int? menuId)    /*//deletin post metodu*/
        {
            if (menuId == null)
            {
                return NotFound();
            }
            MainMenu? dbMainMenu = await _db.MainMenus.FirstOrDefaultAsync(x => x.Id == menuId);
            if (dbMainMenu == null)
            {
                return BadRequest();
            }
            if (dbMainMenu.IsDeactive)
            {
                dbMainMenu.IsDeactive = false;
            }
            else
            {
                dbMainMenu.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region Aktivfood
        public async Task<IActionResult> Aktivfood(int? foodId)    /*//deletin post metodu*/
        {
            if (foodId == null)
            {
                return NotFound();
            }
            Food? dbFood = await _db.Foods.FirstOrDefaultAsync(x => x.Id == foodId);
            if (dbFood == null)
            {
                return BadRequest();
            }
            if (dbFood.IsDeactive)
            {
                dbFood.IsDeactive = false;
            }
            else
            {
                dbFood.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region Detailfood
        public async Task<IActionResult> Detailfood(int? foodId)
        {
            if (foodId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Food? dbFood = await _db.Foods.FirstOrDefaultAsync(x => x.Id == foodId);
            if (dbFood == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbFood);
        }
        #endregion
        //-----------------------------------------------//
    }
}
