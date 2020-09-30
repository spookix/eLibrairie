using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using eLibrairie.Core.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front.Controllers
{
    public class BasketController : Controller
    {
        public const string SessionNbrPanier = "_NbrPanier";
        public const string SessionListBook = "_ListeBook";
        public const string SessionCommande = "_Commande";
        public const string SessionListDetailCommande = "_ListeDetailCommande";
        public const string SessionPrixTotal = "_PrixTotal";

        // GET: Basket
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetBasket(int? id, bool panier, List<int> quantite)
        {
            List<DetailCommande> ListDetailC = HttpContext.Session.Get<List<DetailCommande>>(SessionListDetailCommande);
            Book book = new Book();
            decimal totalPrice = 0;

            if (panier)
            {
                if (id != null)
                {
                    DetailCommande detailC = new DetailCommande();
                    List<Book> bookListLocal = HttpContext.Session.Get<List<Book>>(SessionListBook);


                    ViewBag.nbrPanier = HttpContext.Session.GetInt32(SessionNbrPanier);

                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + id))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            book = JsonConvert.DeserializeObject<Book>(apiResponse);
                        }
                    }
                    if (HttpContext.Session.GetInt32(SessionNbrPanier) == null)
                        HttpContext.Session.SetInt32(SessionNbrPanier, 0);

                    if (bookListLocal == null)
                        bookListLocal = new List<Book>();

                    if (ListDetailC == null)
                        ListDetailC = new List<DetailCommande>();

                    bool exist = false;
                    foreach (var b in bookListLocal)
                        if (b.Id == book.Id)
                            exist = true;
                    if (!exist && book.Id != 0)
                    {
                        bookListLocal.Add(book);
                        detailC.BookId = book.Id;
                        detailC.Quantity = quantite[0];
                        detailC.PrixTotal = quantite[0] * book.Price;
                        ListDetailC.Add(detailC);
                        ViewBag.nbrPanier++;
                        int nbrDansPanier = ViewBag.nbrPanier;

                        HttpContext.Session.Set<List<Book>>(SessionListBook, bookListLocal);
                        HttpContext.Session.Set<List<DetailCommande>>(SessionListDetailCommande, ListDetailC);
                        HttpContext.Session.SetInt32(SessionNbrPanier, nbrDansPanier);

                    }
                    else if (exist && book.Id != 0)
                    {
                        for (int item = 0; item < ListDetailC.Count(); item++)
                            if (ListDetailC.ToList()[item].BookId == book.Id)
                            {
                                if (quantite.ToList()[item] > 0)
                                {
                                    ListDetailC.ToList()[item].Quantity = quantite.ToList()[item];
                                    ListDetailC.ToList()[item].PrixTotal = book.Price * quantite.ToList()[item];
                                }
                            }
                        HttpContext.Session.Set<List<DetailCommande>>(SessionListDetailCommande, ListDetailC);
                    }
                }
            }
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
                totalPrice += book.Price * b.Quantity;
            }

            ViewBag.PrixTotal = totalPrice;

            return View(ListDetailC);
        }

        public async Task<IActionResult> GetDelete(int Id)
        {
            List<DetailCommande> ListDetailC = HttpContext.Session.Get<List<DetailCommande>>(SessionListDetailCommande);
            List<Book> bookListLocal = HttpContext.Session.Get<List<Book>>(SessionListBook);
            //DetailCommande detailC = new DetailCommande();

            decimal totalPrice = 0;
            Book book = new Book();
            ViewBag.nbrPanier = HttpContext.Session.GetInt32(SessionNbrPanier);

            if (HttpContext.Session.GetInt32(SessionNbrPanier) == null)
                HttpContext.Session.SetInt32(SessionNbrPanier, 0);

            if (bookListLocal == null)
                bookListLocal = new List<Book>();

            if (ListDetailC == null)
                ListDetailC = new List<DetailCommande>();

            if (bookListLocal != null && ListDetailC != null && book != null)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + Id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }



                for (int item = 0; item < bookListLocal.Count(); item++)
                    if (bookListLocal.ToList()[item].Id == book.Id)
                    {
                        bookListLocal.RemoveAt(item);
                        ListDetailC.RemoveAt(item);
                        ViewBag.nbrPanier--;
                        int nbrDansPanier = ViewBag.nbrPanier;
                        HttpContext.Session.SetInt32(SessionNbrPanier, nbrDansPanier);
                    }

                HttpContext.Session.Set<List<Book>>(SessionListBook, bookListLocal);
                HttpContext.Session.Set<List<DetailCommande>>(SessionListDetailCommande, ListDetailC);

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
                    totalPrice += book.Price * b.Quantity;
                }
                HttpContext.Session.Set<decimal>(SessionPrixTotal, totalPrice);
            }

            return View(ListDetailC);
        }

    }
}