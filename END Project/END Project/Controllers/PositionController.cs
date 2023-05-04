using END_Project.DAL;
using END_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace END_Project.Controllers
{
    public class PositionController : Controller
    {
        //-----------------------------------------------//
        #region sql
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public PositionController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        //-----------------------------------------------//
        #region İndex
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = Math.Ceiling((decimal)_db.Positions.Count() / 3);
            List<Position> Positions = await _db.Positions.Include(f => f.Employees).Skip((page - 1) * 3).Take(3).ToListAsync();
            return View(Positions);

        }
        #endregion
        //-----------------------------------------------//
        #region CreatePosition
        public IActionResult CreatePosition()
        {
            return View();
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePosition(Position position)
        {

            //if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            //{
            //    return View();

            //}
            bool isExist = await _db.Positions.AnyAsync(x => x.PosName == position.PosName);   //database de var və yox olduğun yoxlamaq üçün
            if (isExist)
            {
                ModelState.AddModelError("Name", "Artıq yaratmısınız");
                return View();
            }

            await _db.Positions.AddAsync(position);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region UpdatePosition
        public async Task<IActionResult> UpdatePosition(int? positionId)
        {
            if (positionId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == positionId);
            if (dbPosition == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePosition(int? positionId, Position position)
        {
            if (positionId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == positionId);
            if (dbPosition == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            //if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            //{
            //    return View(dbPosition);
            //}
            dbPosition.PosName = position.PosName;
            dbPosition.Responsibilities = position.Responsibilities;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region ActivityPosition
        public async Task<IActionResult> ActivityPosition(int? positionId)    /*//deletin post metodu*/
        {
            if (positionId == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == positionId);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            if (dbPosition.IsDeactive)
            {
                dbPosition.IsDeactive = false;
            }
            else
            {
                dbPosition.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region DetailPosition
        public async Task<IActionResult> DetailPosition(int? positionId)
        {
            if (positionId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == positionId);
            if (dbPosition == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        #endregion
        //-----------------------------------------------//
    }
}
