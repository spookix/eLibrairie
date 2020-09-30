using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Front.Models;
using eLibrairie.Core.Data.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Front.Controllers
{
    public class HomeController : Controller
    {
        public const string SessionNbrPanier = "_NbrPanier";
        public const string SessionListBook = "_ListeBook";
        public const string SessionCommande = "_Commande";
        public const string SessionListDetailCommande = "_ListeDetailCommande";
        public const string SessionPrixTotal = "_PrixTotal";
        public const string SessionCompte = "_compte";
        public const string SessionCompteBool = "_compteBool";

        private readonly ILogger<HomeController> _logger;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionNbrPanier) == null)
            {
                HttpContext.Session.SetInt32(SessionNbrPanier, 0);
            }

            if (HttpContext.Session.Get<List<DetailCommande>>(SessionListDetailCommande) == null)
            {
                List<DetailCommande> ListDetailC = new List<DetailCommande>();
                HttpContext.Session.Set<List<DetailCommande>>(SessionListDetailCommande, ListDetailC);
            }

            if (HttpContext.Session.Get<List<Book>>(SessionListBook) == null)
            {
                List<Book> ListBook = new List<Book>();
                HttpContext.Session.Set<List<Book>>(SessionListBook, ListBook);
            }

            //var nbrPanier = HttpContext.Session.GetInt32(SessionNbrPanier);

            List<Book> BookList = new List<Book>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/books"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    BookList = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
            }

            return View(BookList);
        }

        public IActionResult Login()
        {
            ViewBag.BoolCpt = true;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            Compte compte = new Compte();
            HttpContext.Session.Set<bool>(SessionCompteBool, false);
            
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var signInReqult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInReqult.Succeeded)
                {
                    compte.Name = user.UserName;
                    compte.Password = password;
                    compte.Mail = user.Email;

                    HttpContext.Session.Set<Compte>(SessionCompte, compte);
                    HttpContext.Session.Set<bool>(SessionCompteBool, true);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.BoolCpt = false;
            return RedirectToAction("Login");
        }


        public IActionResult Register()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            Compte compte = new Compte();
            HttpContext.Session.Set<bool>(SessionCompteBool, false);
            var user = new IdentityUser
            {
                UserName = username,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var signInReqult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInReqult.Succeeded)
                {
                    compte.Id = 1;  // a modifier une fois le compte créé dans la bdd
                    compte.Name = user.ToString();
                    compte.Password = password;
                    compte.Mail = user.Email;

                    HttpContext.Session.Set<Compte>(SessionCompte, compte);
                    HttpContext.Session.Set<bool>(SessionCompteBool, true);
                    return RedirectToAction("index");
                }
            }
            return RedirectToAction("Register");
        }

        public IActionResult Deconnexion()
        {
            HttpContext.Session.Set<bool>(SessionCompteBool, false);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> CommandeHistory()
        {
            Compte compte = HttpContext.Session.Get<Compte>(SessionCompte);
            List<Commande> CommandeList = new List<Commande>();
            List<Commande> CommandeListCompte = new List<Commande>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/commandes"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CommandeList = JsonConvert.DeserializeObject<List<Commande>>(apiResponse);
                }

            }
            foreach (var com in CommandeList)
                if (com.CompteMail == compte.Mail)
                    CommandeListCompte.Add(com);

            ViewBag.Compte = compte;
            return View(CommandeListCompte);
        }

        public async Task<IActionResult> CommandeDetailHistory(int Id)
        {
            List<DetailCommande> receevedListDetailC = new List<DetailCommande>();
            List<DetailCommande> listDetailC = new List<DetailCommande>();
            Commande commande = new Commande();
            List<Book> listBook = new List<Book>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/detailCommandes"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receevedListDetailC = JsonConvert.DeserializeObject<List<DetailCommande>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("http://localhost:53939/api/books"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listBook = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("http://localhost:53939/api/commandes/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    commande = JsonConvert.DeserializeObject<Commande>(apiResponse);
                }
            }

            foreach (var detail in receevedListDetailC)
                if (detail.CommandeId == commande.Id)
                    listDetailC.Add(detail);

            ViewBag.ListeDetailsCommandes = listDetailC;
            ViewBag.ListeBooks = listBook;
            ViewBag.PrixTotal = commande.Price;

            return View();
        }

        public async Task<IActionResult> AffichageCompte()
        {
            Compte compte = HttpContext.Session.Get<Compte>(SessionCompte);
            List<Commande> listCommandes = new List<Commande>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/commandes"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listCommandes = JsonConvert.DeserializeObject<List<Commande>>(apiResponse);
                }
            }

            int nbrCommande = 0;

            foreach (var com in listCommandes)
                if (com.CompteMail == compte.Mail)
                    nbrCommande++;

            ViewBag.NbrCommande = nbrCommande;

            return View(compte);
        }

        public IActionResult EditCompte()
        {
            Compte compte = HttpContext.Session.Get<Compte>(SessionCompte);

            return View(compte);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompte(Compte compte)
        {
            Compte compteOrigine = HttpContext.Session.Get<Compte>(SessionCompte);
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(compteOrigine.Name);

                user.UserName = compte.Name;
                user.Email = compte.Mail;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    compteOrigine.Name = compte.Name;
                    compteOrigine.Mail = compte.Mail;
                    HttpContext.Session.Set<Compte>(SessionCompte, compteOrigine);
                }


                return RedirectToAction("AffichageCompte");
            }

            return View(compte);
        }

        public IActionResult ModifMdp()
        {
            Compte compte = HttpContext.Session.Get<Compte>(SessionCompte);
            ViewBag.MdpDiff = true;
            return View(true);
        }

        [HttpPost]
        public async Task<IActionResult> ModifMdp(string mdpBase, string mdpChangeUn, string mdpChangeDeux)
        {
            Compte compte = HttpContext.Session.Get<Compte>(SessionCompte);
            bool valide = false;
            var user = await _userManager.FindByNameAsync(compte.Name);

            if (mdpBase != null && mdpChangeUn != null && mdpChangeDeux != null)
            {
                if (mdpBase == compte.Password)
                {
                    valide = true;
                    if (mdpChangeUn == mdpChangeDeux)
                    {
                        ViewBag.MdpDiff = true;
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, mdpChangeUn);
                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            compte.Password = mdpChangeUn;
                            HttpContext.Session.Set<Compte>(SessionCompte, compte);
                            return RedirectToAction("MdpModifie");
                        }
                        else
                        {
                            ViewBag.MdpDiff = false;
                        }
                    }
                    else
                    {
                        ViewBag.MdpDiff = false;
                    }
                }
            }
            ViewBag.MdpDiff = false;
            return View(valide);
        }

        public IActionResult MdpModifie()
        {
            return View();
        }

        //public IActionResult SuppCompte()
        //{
        //    return View();
        //}

        public async Task<IActionResult> SuppCompte(bool supp)
        {
            Compte compte = HttpContext.Session.Get<Compte>(SessionCompte);
            var user = await _userManager.FindByNameAsync(compte.Name);

            if (supp)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    compte = new Compte();
                    List<DetailCommande> ListDetailC = new List<DetailCommande>();
                    List<Book> listBook = new List<Book>();
                    HttpContext.Session.Set<List<Book>>(SessionListBook, listBook);
                    HttpContext.Session.Set<List<DetailCommande>>(SessionListDetailCommande, ListDetailC);
                    HttpContext.Session.Set<Compte>(SessionCompte, compte);
                    HttpContext.Session.Set<bool>(SessionCompteBool, false);
                    HttpContext.Session.Set(SessionNbrPanier, 0);
                    HttpContext.Session.SetInt32(SessionPrixTotal, 0);
                    return RedirectToAction("Index");
                }
            }
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
