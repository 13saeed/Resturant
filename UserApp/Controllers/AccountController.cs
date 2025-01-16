using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Resturant.Models;
using Resturant.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Resturant.Controllers
{
   
    

    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
                {
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var user = await userManager.FindByEmailAsync(model.Email);
                        if (user != null)
                        {
                            var roles = await userManager.GetRolesAsync(user);
                            if (roles.Contains("Admin"))
                            {
                                return RedirectToAction("DashboardAdmin", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "User not found");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email or Password is incorrect");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email and Password are required");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", "Password is required");
                    return View(model);
                }

                Users users = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await userManager.CreateAsync(users, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViweModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViweModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        var result = await userManager.RemovePasswordAsync(user);
                        if (result.Succeeded)
                        {
                            result = await userManager.AddPasswordAsync(user, model.NewPassword);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("Login", "Account");
                            }
                            else
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError("", error.Description);
                                }
                                return View(model);
                            }
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "New Password is required");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. Try again");
                return View(model);
            }
        }

       


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
