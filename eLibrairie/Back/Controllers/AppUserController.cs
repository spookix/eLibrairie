using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    public class AppUserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AppUserController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profil(string username)
        {
            this.ViewBag.currentIndexSideBare = "profil";
            this.ViewBag.currentIndexMenu = "profil";

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {

                return View(user);
            }
            return View();
        }

        public async Task<IActionResult> Edit(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                return View(user);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string username, string password, string firstname, string lastname)
        {
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
               
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

            //Je returne sur Index tant que je ne sais pas encore faire un message flash
        }
    }
}