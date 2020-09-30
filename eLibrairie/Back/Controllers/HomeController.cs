using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Back.Models;
using eLibrairie.Core.Data.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Back.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {

            // pour mettre a jour le menu courant
            this.ViewBag.currentIndexMenu = "home";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            List<Book> BookList = new List<Book>();
            List<Categorie> CategorieList = new List<Categorie>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/books"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    BookList = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("http://localhost:53939/api/categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CategorieList = JsonConvert.DeserializeObject<List<Categorie>>(apiResponse);
                }
            }
            this.ViewBag.numBooks = BookList.Count;
            this.ViewBag.numCategories = CategorieList.Count;

            // pour la mise à jour de la sidebar
            this.ViewBag.currentIndexSideBare = "dashboard";
            this.ViewBag.currentIndexMenu = "dashboard";
            // pour la date du jour
            this.ViewBag.theDate = DateTime.Now;

            return View(BookList);
        }


        public IActionResult Login()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var signInReqult = await _signInManager.PasswordSignInAsync(user, password,false,false);
                if (signInReqult.Succeeded) {
                    return RedirectToAction("Dashboard");
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult Register()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string firstname, string lastname)
        {
            AppUser user = await _userManager.FindByNameAsync(username);
            if( user == null )
            {
                user = new AppUser
                {
                    UserName = username,
                    Email = "",
                    FirstName = firstname,
                    LastName = lastname
                };
                
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var signInReqult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    if (signInReqult.Succeeded)
                    {
                        return RedirectToAction("index");
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

            //Je returne sur Index tant que je ne sais pas encore faire un message flash
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

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
