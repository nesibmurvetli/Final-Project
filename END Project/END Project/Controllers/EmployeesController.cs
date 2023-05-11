using END_Project.DAL;
using END_Project.Helpers;
using END_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace END_Project.Controllers
{
    public class EmployeesController : Controller
    {
        //-----------------------------------------------//
        #region sql
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public EmployeesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        //-----------------------------------------------//
        #region İndex
        public async Task<IActionResult> Index()
        {
            List<Employee> Employees = await _db.Employees.Include(x => x.Position).ToListAsync();
            
            return View(Employees);
        }
        #endregion
        //-----------------------------------------------//
        #region CreateEmployee
        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.Positions = await _db.Positions.Where(x=>!x.IsDeactive).ToListAsync();
            return View();
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(Employee employee, int positionId,bool gender)
        {
            ViewBag.Positions = await _db.Positions.Where(x => !x.IsDeactive).ToListAsync();

            bool isExist = await _db.Employees.AnyAsync(x => x.Name == employee.Name);   //database de var və yox olduğun yoxlamaq üçün
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu artıq mövcuddur");
                return View();
            }
            if (employee.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            if (!employee.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin");
                return View();
            }
            if (employee.Photo.Max2Mb())
            {
                ModelState.AddModelError("Photo", "Şəkilin ölçüsü maxsimal olaraq 2mb olmalıdır");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "assets", "projecphotos");
            employee.Image = await employee.Photo.SaveFileAsync(folder);
            employee.IsMale=gender;
            employee.PositionId = positionId;
            await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region AktivEmployee
        public async Task<IActionResult> AktivEmployee(int? employeeId)    /*//deletin post metodu*/
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            Employee? dbEmployee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            if (dbEmployee.IsDeactive)
            {
                dbEmployee.IsDeactive = false;
            }
            else
            {
                dbEmployee.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region DetailEmployee
        public async Task<IActionResult> DetailEmployee(int? employeeId)
        {
            if (employeeId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Employee? dbEmployee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
            if (dbEmployee == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbEmployee);
        }
        #endregion
        //-----------------------------------------------//
        #region UpdateEmployee
        public async Task<IActionResult> UpdateEmployee(int? employeeId)
        {
            if (employeeId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Employee? dbEmployee = await _db.Employees.Include(x => x.Position).FirstOrDefaultAsync(x => x.Id == employeeId);
            if (dbEmployee == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            ViewBag.Positions = await _db.Positions.ToListAsync();
            return View(dbEmployee);
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(int? employeeId, Employee employee, int positionId,bool gender)
        {
            if (employeeId == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Employee? dbEmployee = await _db.Employees.Include(x => x.Position).FirstOrDefaultAsync(x => x.Id == employeeId);
            if (dbEmployee == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            //if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            //{
            //    return View(dbEmployee);
            //}
            if (employee.Photo != null)
            {
                if (!employee.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa bir şəkil fayli seçin");
                    return View(dbEmployee);
                }
                if (!employee.Photo.Max2Mb())
                {
                    ModelState.AddModelError("Photo", "Şəkil max. 2 mb ola bilər");
                    return View(dbEmployee);
                }

                string folder = Path.Combine(_env.WebRootPath, "assets", "projecphotos");
                employee.Image = await employee.Photo.SaveFileAsync(folder);
                dbEmployee.Image = employee.Image;
            }
            dbEmployee.IsMale = gender;
            dbEmployee.Name = employee.Name;
            dbEmployee.Surname = employee.Surname;
            dbEmployee.FatherName = employee.FatherName;
            dbEmployee.DateofBirth = employee.DateofBirth;
            dbEmployee.PositionId = employee.PositionId = positionId;
            dbEmployee.PhoneNumber = employee.PhoneNumber;
            dbEmployee.Email = employee.Email;
            dbEmployee.Salary = employee.Salary;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
        #region SendMail
        public async Task<IActionResult> SendMail(int? employeeId)
        {
            Employee? employee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
            if (employee == null)
            {
                return BadRequest();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMail(EmailMessage emailMessage,int? employeeId)
        {
            Employee? employee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
            if (employee == null)
            {
                return BadRequest();
            }
            emailMessage.To = employee.Email;
            try
            {
                    await Helper.SendMail(messageSubject: emailMessage.Subject,emailMessage.Body, emailMessage.To);

            }
            catch
            {


            }
            return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------//
    }
}
