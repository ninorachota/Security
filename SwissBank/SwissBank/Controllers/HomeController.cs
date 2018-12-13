using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using SwissBank.Data;
using SwissBank.Models;
using SwissBank.Services;

namespace SwissBank.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;

        public HomeController(UserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_userService.GetCurrentIdentityUser().Result);
        }

        public IActionResult ConfirmEmail()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}