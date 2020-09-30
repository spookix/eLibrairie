using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eLibrairie.Core.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front.Controllers
{
    public class CommandeController : Controller
    {
        public const string SessionCompte = "_compte";
        public const string SessionCompteBool = "_compteBool";
        public const string SessionListBook = "_ListeBook";
        public const string SessionListDetailCommande = "_ListeDetailCommande";
        public const string SessionPrixTotal = "_PrixTotal";
        public const string SessionNbrPanier = "_NbrPanier";
        public const string SessionListPrix = "_ListePrix";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Commande()
        {
            if (HttpContext.Session.Get<bool>(SessionCompteBool))
                return RedirectToAction("Paiement");
            else
                return RedirectToAction("Compte");
        }

        public IActionResult Compte()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Compte(Compte compte)
        {
            Compte newCompte = new Compte();
            if (compte != null)
            {
                newCompte.Name = compte.Name;
                newCompte.Firstname = compte.Firstname;
                newCompte.Mail = compte.Mail;
                newCompte.Id = 1;
                HttpContext.Session.Set<Compte>(SessionCompte, newCompte);
            }

            return RedirectToAction("Paiement");

        }

        public async Task<IActionResult> Paiement()
        {
            decimal PrixTotal = 0;
            Book book = new Book();
            List<DetailCommande> ListDetailC = HttpContext.Session.Get<List<DetailCommande>>(SessionListDetailCommande);

            // Changer CompteId commande et detail quand compte ajouté a la bdd

            foreach (var b in ListDetailC)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + b.BookId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }
                PrixTotal += book.Price * b.Quantity;
            }
            ViewBag.PrixTotal = PrixTotal;
            return View();
        }

        public async Task<IActionResult> FactureAsync()
        {
            //List<Book> bookListLocal = HttpContext.Session.Get<List<Book>>(SessionListBook);
            List<DetailCommande> ListDetailC = HttpContext.Session.Get<List<DetailCommande>>(SessionListDetailCommande);

            List<decimal> listPrix = new List<decimal>();
            Book book = new Book();

            foreach (var b in ListDetailC)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + b.BookId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }
                listPrix.Add(book.Price * b.Quantity);
            }

            //ViewBag.ListPrix = listPrix;

            return View(listPrix);
        }

        [HttpPost]
        public async Task<IActionResult> Facture()
        {
            Compte compte = HttpContext.Session.Get<Compte>(SessionCompte);
            Commande commande = new Commande();
            Commande receivedCommande = new Commande();
            //List<DetailCommande> receivedListDetailC = new List<DetailCommande>();
            List<Book> listBook = HttpContext.Session.Get<List<Book>>(SessionListBook);
            //List<decimal> listPrix = new List<decimal>();
            Book book = new Book();
            decimal PrixTotal = 0;

            List<DetailCommande> ListDetailC = HttpContext.Session.Get<List<DetailCommande>>(SessionListDetailCommande);

            // Changer CompteId commande et detail quand compte ajouté a la bdd

            foreach (var b in ListDetailC)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + b.BookId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }
                PrixTotal += book.Price * b.Quantity;
            }


            commande.CompteMail = compte.Mail;
            commande.Price = PrixTotal;
            commande.Date = DateTime.Now;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(commande), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:53939/api/commandes", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedCommande = JsonConvert.DeserializeObject<Commande>(apiResponse);
                }
            }

            foreach (var detailC in ListDetailC)
            {
                detailC.CommandeId = receivedCommande.Id;
                detailC.Id = 0;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(detailC), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("http://localhost:53939/api/DetailCommandes", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        //receivedListDetailC = JsonConvert.DeserializeObject<List<DetailCommande>>(apiResponse);
                    }
                }
                //PrixTotal += detailC.PrixTotal;
            }

            //HttpContext.Session.Set<List<decimal>>(SessionListPrix, listPrix);
            ViewBag.Commandes = receivedCommande;
            ViewBag.ListeDetailsCommandes = ListDetailC;
            ViewBag.ListeBooks = listBook;
            ViewBag.PrixTotal = PrixTotal;
            //ViewBag.ListePrix = listPrix;

            ListDetailC = new List<DetailCommande>();
            listBook = new List<Book>();
            HttpContext.Session.Set<List<Book>>(SessionListBook, listBook);
            HttpContext.Session.Set<List<DetailCommande>>(SessionListDetailCommande, ListDetailC);
            HttpContext.Session.SetInt32(SessionNbrPanier, 0);
            HttpContext.Session.SetInt32(SessionPrixTotal, 0);

            //return RedirectToAction("Facture");



            return View();
        }

    }
}