using BankAppData.Models;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BankWebbApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BankAppDataContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, BankAppDataContext db, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddApplicationUser()
        {
            var model = new AplicationUser();
            var roles = _roleManager.Roles.ToList();
            model.AllRoles = roles.Select(x => new Roles
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,

            }).ToList();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddApplicationUser(AplicationUser model)
        {

            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    PasswordHash = model.Password,
                    NormalizedEmail = model.ConfirmPassword,
                    UserName = model.Email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRolesAsync(user, model.AllRoles.Where(x => x.IsSelected == true).Select(x => x.Name).ToList());
                await _db.SaveChangesAsync();
                return RedirectToAction("index");

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ApplicationUsers()
        {

            var model = _userManager.Users.Select(x => new ApplicationUsersDetails
            {
                Email = x.Email,
                Id = x.Id,

            }).ToList();

            foreach (var applicationusermodel in model)
            {

                var results = await _userManager.GetRolesAsync(new IdentityUser { Id = applicationusermodel.Id });
                foreach (var result in results)
                {
                    applicationusermodel.AllRoles.Add(new IdentityRole { Name = result });
                }

            }

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> EditApplicationUser(string Id)
        {
            var model = new EditApplicationUser();
            var user = _userManager.Users.FirstOrDefault(x => x.Id == Id);
            var result = await _userManager.GetRolesAsync(user);

            var roles = _roleManager.Roles.ToList();
            model.AllRoles = roles.Select(x => new Roles
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                IsSelected = result.Any(j => j == x.Name)
            }).ToList();
            model.Email = user.Email;
            model.Password = "";
            model.ConfirmPassword = "";
            model.Id = Id;
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplicationUser(EditApplicationUser model)
        {

            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == model.Id);
                var result = await _userManager.GetRolesAsync(user);
                user.Email = model.Email;
                user.UserName = model.Email;
                user.NormalizedUserName = model.Email;
                user.NormalizedEmail = model.Email;
                foreach (var item in model.AllRoles)
                {
                    if (!result.Any(x => x == item.Name) && item.IsSelected == true)
                    {
                        await _userManager.AddToRoleAsync(user, item.Name);
                    }

                    if (result.Any(x => x == item.Name) && item.IsSelected == false)
                    {
                        await _userManager.RemoveFromRoleAsync(user, item.Name);
                    }

                }
                if (model.Password != null && model.ConfirmPassword != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, model.Password);
                }
                await _userManager.UpdateNormalizedUserNameAsync(user);
                await _userManager.UpdateNormalizedEmailAsync(user);
                await _db.SaveChangesAsync();
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                if (currentuser == user)
                {
                    await _signInManager.SignOutAsync();
                }
                model.SucessMessage = "User have been edited successfully ";
            }
            return View(model);

        }
        public async Task<IActionResult> DeleteApplicationUsers(string Id)
        {

            var user = _userManager.Users.FirstOrDefault(x => x.Id == Id);

            var result = await _userManager.GetRolesAsync(user);
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
            }
            await _userManager.DeleteAsync(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("ApplicationUsers");

        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyApplicationUserEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;

            if (user != null)
            {
                return Json("Email is already exsist");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEditApplicationUserEmail(string email, string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var userexsist = _userManager.FindByEmailAsync(email).Result;

            if (user.Email == email && user.Id == id)
            {
                return Json(true);

            }
            else if (userexsist != null)
            {
                return Json("Email is already exsist");
            }
            return Json(true);

        }
    }
}