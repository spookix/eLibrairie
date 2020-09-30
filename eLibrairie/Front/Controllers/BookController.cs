using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using eLibrairie.Core.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front.Controllers
{
    public class BookController : Controller
    {
        public const string SessionNbrPanier = "_NbrPanier";
        public const string SessionListBook = "_ListeBook";
        public const string SessionCommande = "_Commande";
        public const string SessionListDetailCommande = "_ListeDetailCommande";
        public const string SessionPrixTotal = "_PrixTotal";
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetBook(int id, bool panier)
        {
            DetailCommande detailC = new DetailCommande();

            List<Book> bookListLocal = HttpContext.Session.Get<List<Book>>(SessionListBook);
            //Commande commande = HttpContext.Session.Get<Commande>(SessionCommande);
            List<DetailCommande> ListDetailC = HttpContext.Session.Get<List<DetailCommande>>(SessionListDetailCommande);
            decimal totalPrice = 0;
            Book book = new Book();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }

            if (panier)
            {
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
                    detailC.Quantity = 1;
                    detailC.PrixTotal = book.Price;
                    ListDetailC.Add(detailC);
                    ViewBag.nbrPanier = this.HttpContext.Session.GetInt32(SessionNbrPanier);
                    ViewBag.nbrPanier++;
                    int nbrDansPanier = this.ViewBag.nbrPanier;

                    HttpContext.Session.Set<List<Book>>(SessionListBook, bookListLocal);
                    HttpContext.Session.Set<List<DetailCommande>>(SessionListDetailCommande, ListDetailC);
                    HttpContext.Session.SetInt32(SessionNbrPanier, nbrDansPanier);
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
                HttpContext.Session.Set<decimal>(SessionPrixTotal, totalPrice);
            }

            return View(book);
        }

        public async Task<IActionResult> GetCategory()
        {
            List<Book> BookList = new List<Book>();
            List<Categorie> CategorieList = new List<Categorie>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/Categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CategorieList = JsonConvert.DeserializeObject<List<Categorie>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("http://localhost:53939/api/Books"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    BookList = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
                this.ViewBag.Categories = CategorieList;
                this.ViewBag.Books = BookList;
            }
            return View();
        }

        public async Task<IActionResult> GetCategoryId(int id)
        {
            List<Book> bookList = new List<Book>();
            List<Book> bookListId = new List<Book>();
            List<Categorie> categorieList = new List<Categorie>();
            //Categorie categorie = new Categorie();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/Categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categorieList = JsonConvert.DeserializeObject<List<Categorie>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("http://localhost:53939/api/Books"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bookList = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
                foreach (Book bk in bookList)
                {
                    if (bk.CategorieId == id)
                    {
                        bookListId.Add(bk);
                    }
                }

                this.ViewBag.Categories = categorieList;
                this.ViewBag.Books = bookListId;
            }
            return View();
        }
        public async Task<HttpStatusCode> Delete(int Id)
        {
            List<Book> bookListLocal = HttpContext.Session.Get<List<Book>>(SessionListBook);
            Book book = new Book();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }

            if (bookListLocal != null && book != null)
                bookListLocal.Remove(book);

            HttpContext.Session.Set<List<Book>>(SessionListBook, bookListLocal);

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:53939/api/books/" + Id.ToString());

                return response.StatusCode;

            }
        }

        public async Task<IActionResult> GetBookByName(string name)
        {
            List<Book> bookList = new List<Book>();
            List<Book> bookListView = new List<Book>();

            if (name != null)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:53939/api/books"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        bookList = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                    }
                }

                foreach (var book in bookList)
                    if (book.Name.Contains(name))
                        bookListView.Add(book);

            }
            ViewBag.bookList = bookListView;

            return View();
        }
    }
}