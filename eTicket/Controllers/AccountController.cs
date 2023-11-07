using eTicket.Data;
using eTicket.Data.Static;
using eTicket.Data.ViewModels;
using eTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace eTicket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Login() => View(new LoginRegisterVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginRegisterVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.LoginModel.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.LoginModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.LoginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }
                TempData["Error"] = "Wrong Credentials.";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong Credentials.";
            return View(loginVM);
        }

        public IActionResult Register() => View(new LoginRegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(LoginRegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.RegisterModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "Email Address already exists!";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.RegisterModel.FullName,
                Email = registerVM.RegisterModel.EmailAddress,
                UserName = registerVM.RegisterModel.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.RegisterModel.Password);

            if (newUserResponse.Succeeded == false)
            {
                var errorMessage = newUserResponse.Errors.ElementAt(0).Description;
                TempData["Error"] = errorMessage;
                return View(registerVM);
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return RedirectToAction("Index", "Movies");
            }

            
            
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
