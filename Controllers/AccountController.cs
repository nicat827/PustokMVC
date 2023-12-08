using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokApp.Models;
using PustokApp.Utilities.Enums;
using PustokApp.ViewModels;
using System.Configuration;
using System.Security.Principal;

namespace PustokApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction(nameof(Login));

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserInfoVM userData = new UserInfoVM
            {
                Email = user.Email,
                Name = user.Name,
                Gender = user.Gender,
                Surname = user.Surname,
                Username = user.UserName
            };
            return View(userData);
        }
       
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM userVM, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }
            bool isValidGender = false;
            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
            {
                if (userVM.Gender == gender)
                {
                    isValidGender = true;
                    break;
                }
            }

            if (!isValidGender)
            {
                ModelState.AddModelError("Gender", "Gender is not valid!");
                return View(userVM);
            }

            RegexStringValidator regex = new RegexStringValidator("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            try
            {
                regex.Validate(userVM.Email);
            }
            catch
            {
                ModelState.AddModelError("Email", "Email is not valid!");
                return View(userVM);
            }

            AppUser user = new AppUser
            {
                Name = userVM.Name,
                Surname = userVM.Surname,
                Gender = userVM.Gender,
                UserName = userVM.Username,
                Email = userVM.Email,

            };

            IdentityResult res = await _userManager.CreateAsync(user, userVM.Password);

            if (!res.Succeeded)
            {
                foreach (IdentityError err in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

                return View(userVM);
            }
            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
            await _signInManager.SignInAsync(user, false);

            if (Request.Cookies["basket"] is not null)
            {
                Response.Cookies.Delete("basket");
            }
            if (returnUrl is not null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
            

        }

        public async Task<IActionResult> Logout(string? returnUrl)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl is not null) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
       
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM userVM, string? returnUrl)
        {
            if (!ModelState.IsValid) return View(userVM);
            AppUser user = await _userManager.FindByNameAsync(userVM.UserNameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(userVM.UserNameOrEmail);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Username, Email or Password is incorrect!");
                    return View();
                }
            }

            var res = await _signInManager.PasswordSignInAsync(user, userVM.Password, userVM.IsRemembered, true);

            if (res.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Too many failed attempts, You have been blocked to 3 minutes!");
                return View();
            }
            if (!res.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username, Email or Password is incorrect!");
                return View();
            }
            if (Request.Cookies["basket"] is not null)
            {
                Response.Cookies.Delete("basket");
            }

            if (returnUrl is not null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
            
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (! await _roleManager.RoleExistsAsync(role.ToString())) {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString()});
                }
            }
            return Ok("Succefully created");
        }


    }
}
