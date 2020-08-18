using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppFour.Models;
using Microsoft.AspNetCore.Identity;

namespace AppFour.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> signInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signinMgr)
        {
            _logger = logger;
            signInManager = signinMgr;
        }

        public async Task<IActionResult> Index()
        {
            await signInManager.SignOutAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
