using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppFour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppFour.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public UserController(UserManager<AppUser> usrMgr, SignInManager<AppUser> signinMgr)
        {
            userManager = usrMgr;
            signInManager = signinMgr;
        }
        
        public IActionResult MainPage()
        {
            return View(userManager.Users);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                appUser.LatestLoginDate = DateTime.Now.ToString();
                if (appUser != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        await userManager.UpdateAsync(appUser);
                        return Redirect(login.ReturnUrl ?? "/User/MainPage");
                    }
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Registration() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegistration userReg)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = userReg.Name,
                    Email = userReg.Email,
                    RegistrationDate = DateTime.Now.ToString(),
                    LatestLoginDate = DateTime.Now.ToString(),
                    Status = true
                };

                IdentityResult result = await userManager.CreateAsync(appUser, userReg.Password);
                if (result.Succeeded)
                {
                    return View("MainPage", userManager.Users);
                }
                else{
                    foreach (IdentityError err in result.Errors)
                        ModelState.AddModelError("", err.Description);
                }
            }
            return View(userReg);
        }

        public IActionResult TableAction(string button, string[] idsArr)
        {
            if (idsArr == null || idsArr.Length == 0 || button == "")
            {
                ModelState.AddModelError("", "No item selected to delete");
                return View("MainPage", userManager.Users);
            }
            else
            {
                if (button.Equals("Block selected"))
                {
                    return RedirectToAction("Block", "User", new { ids = idsArr });
                }
                else if (button.Equals("Unblock selected"))
                {
                    return RedirectToAction("Unblock", "User", new { ids = idsArr });
                }
                else if(button.Equals("Delete selected"))
                {
                    return RedirectToAction("Delete", "User", new { ids = idsArr });
                }
                else
                    return View("MainPage", userManager.Users);
            }
        }

        public async Task<IActionResult> Block(string[] ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                AppUser usr = await userManager.FindByIdAsync(ids[i]);
                usr.Status = false;
                await userManager.UpdateAsync(usr);
            }
            return View("MainPage", userManager.Users);
        }
        public async Task<IActionResult> UnBlock(string[] ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                AppUser usr = await userManager.FindByIdAsync(ids[i]);
                usr.Status = true;
                await userManager.UpdateAsync(usr);
            }
            return View("MainPage", userManager.Users);
        }

        public async Task<IActionResult> Delete(string[] ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                AppUser usr = await userManager.FindByIdAsync(ids[i]);
                await userManager.DeleteAsync(usr);
            }
            return View("MainPage", userManager.Users);
        }
    }
}
