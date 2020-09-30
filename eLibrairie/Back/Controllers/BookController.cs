using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eLibrairie.Core.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Back.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        public async Task<IActionResult> Index(int page = 0, int size = 5)
        {         
            List<Categorie> CategorieList = new List<Categorie>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CategorieList = JsonConvert.DeserializeObject<List<Categorie>>(apiResponse);
                }
            }
            this.ViewBag.CategorieList = CategorieList;

            List<Book> BookList = new List<Book>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/books"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    BookList = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
            }
            // Pour la pagination
            int position = page * size;
            decimal totalPages = Math.Ceiling( (decimal)BookList.Count/size );
            this.ViewBag.currentPage = page;
            this.ViewBag.totalPages = totalPages;

            // pour mettre a jour le menu courant
            this.ViewBag.currentIndexSideBare = "book";
            this.ViewBag.currentIndexMenu = "book";
            return View(BookList.Skip(position).Take(size));
        }

        public async Task<IActionResult> Create()
        {
            List<Categorie> CategorieList = new List<Categorie>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CategorieList = JsonConvert.DeserializeObject<List<Categorie>>(apiResponse);
                }
            }
            this.ViewBag.CategorieList = CategorieList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                Book receivedBook = new Book();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("http://localhost:53939/api/books", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedBook = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(int id)
        {
            List<Categorie> CategorieList = new List<Categorie>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    CategorieList = JsonConvert.DeserializeObject<List<Categorie>>(apiResponse);
                }
            }

            //pour le select
            this.ViewBag.CategorieList = CategorieList;
            

            Book book = new Book();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/books/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    book = JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    //  Book theBook = null;
                    using (var response = await httpClient.PutAsJsonAsync("http://localhost:53939/api/books/" + book.Id.ToString(), book))
                    {
                        response.EnsureSuccessStatusCode();
                        //  theBook = await response.Content.ReadAsAsync<Categorie>();
                    }

                    return RedirectToAction("Index");
                }
            }

            return View(book);
        }

        public async Task<HttpStatusCode> Delete(int Id)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync("http://localhost:53939/api/books/" + Id.ToString());

                //return RedirectToAction("Index");
                return response.StatusCode;

            }
        }

        
    }
}