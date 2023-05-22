using END_Project.Helpers;
using END_Project.Models;
using END_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace END_Project.Controllers
{
    public class AccountController : Controller
    {
        //-----------------------------------------------//
        #region userlerin istifadsi ucun
        private readonly UserManager<AppUser> _userManager; /* userlerin idarəsi üçün ist olunur*/
        private readonly RoleManager<IdentityRole> _roleManager;/* burda konkret obyekt yaratmışıq*/
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
                                   RoleManager<IdentityRole> roleManager,
                                     SignInManager<AppUser> signInManager)
        {
            _userManager = userManager; /* userləriidarəedən kod*/
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        #endregion
        //-----------------------------------------------//
        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser? appUser = await _userManager.FindByNameAsync(loginVM.Username);
            if (appUser == null)
            {
                ModelState.AddModelError("", "İstifadəçi adı və şifrə səhvdir!");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut) /*giriş koduna qoyulan limiti aşdıqda çıxan error kodu*/
            {
                ModelState.AddModelError("", "Limiti keçmisiniz");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "İstifadəçi adı və şifrə səhvdir!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        //-----------------------------------------------//
        #region register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) /*xanalar tam doldurulmayanda verilen xetanı eks eletdirir*/
            {
                return View();
            }
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, password: registerVM.Password); /*qeydiyyatdan kecmek ucun*/
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(appUser, true);  /*Burdaki true yaddaaşda qalsın mi funksiyası üçündü*/
            await _userManager.AddToRoleAsync(appUser, Helper.Roles.Member.ToString());

            return RedirectToAction("Login", "Account"
              /*  "Index", "Home"*/);
        }
        #endregion
        //-----------------------------------------------//
        #region Logout
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
                //return NotFound();
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        #endregion
        //-----------------------------------------------//
        //#region CreateRole
        //public async Task CreateRole()
        //{
        //    if (!(await _roleManager.RoleExistsAsync(Helper.Roles.SuperAdmin.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.SuperAdmin.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Admin.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Admin.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Helper.Roles.Member.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Member.ToString() });
        //    }
        //}
        //#endregion
        //-----------------------------------------------//
    }

}