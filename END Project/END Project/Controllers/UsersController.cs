﻿using END_Project.Models;
using END_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace END_Project.Controllers
{
    //[Authorize(Roles = "SuperAdmin ,Admin")]
    public class UsersController : Controller
    {
        #region db den
        private readonly UserManager<AppUser> _userManager; /*userleriidarəetməkucun*/
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync(); /*Sql  den gelen  dataları tək tək şəkmək*/
            List<UserVM> userVMs = new List<UserVM>(); /*tək tək çəkdiklərimizi  bir yeni yere yığırıq*/
            foreach (AppUser user in users)
            //"user" ler "users" ın icindeki tek tek userlerdi
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Username = user.UserName,
                    IsDeactive = user.IsDeactive,
                    IsMale = user.IsMale,
                    PhoneNumber = user.PhoneNumber,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        #endregion
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM, string newRole)
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, password: registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(appUser, newRole);
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(string id, bool gender)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM updateVM = new UpdateVM
            {
                IsMale=user.IsMale,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
            };
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(updateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM, string newRole, bool gender)
        {
            #region Fromget
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName,
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            #endregion
            if (!ModelState.IsValid)
            {
                return View(dbUpdateVM);
            }
            user.Email = updateVM.Email;
            user.UserName = updateVM.Username;
            user.Name = updateVM.Name;
            user.Surname = updateVM.Surname;
            user.PhoneNumber = updateVM.PhoneNumber;
            user.IsMale = gender;
            await _userManager.UpdateAsync(user);/* userlerde datanı dəyişəndə sql e gpndermek kodu*/
            if (newRole != dbUpdateVM.Role)    /*yeni rolu deyişmədikdə*/
            {
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(user, newRole);
                if (!addIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(user, role: dbUpdateVM.Role);
                if (!removeIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Aktiv
        public async Task<IActionResult> Activity(string id)    /*//deletin post metodu*/
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser? dbUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            if (dbUser.IsDeactive)
            {
                dbUser.IsDeactive = false;
            }
            else
            {
                dbUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(dbUser);
            return RedirectToAction("Index");
        }
        #endregion
        #region ResetPasswordVM
        //kod deyişmək üçün
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, ResetPasswordVM resetPasswordVM, string newRole)
        {
            #region Fromget
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            #endregion
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Detail
        public async Task<IActionResult> Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            return View(user);
        }
        #endregion
    }
}
