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
    public class CategorieController : Controller
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

            // Pour la pagination
            int position = page * size;
            decimal totalPages = Math.Ceiling((decimal)CategorieList.Count / size);
            this.ViewBag.currentPage = page;
            this.ViewBag.totalPages = totalPages;

            // pour mettre a jour le menu et la sidebar
            this.ViewBag.currentIndexSideBare = "categorie";
            this.ViewBag.currentIndexMenu = "categorie";
            return View(CategorieList.Skip(position).Take(size));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                Categorie receivedCategorie = new Categorie();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(categorie), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("http://localhost:53939/api/categories", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedCategorie = JsonConvert.DeserializeObject<Categorie>(apiResponse);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(categorie);
        }

        public async Task<IActionResult> Edit(int id)
        {

            Categorie categorie = new Categorie();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:53939/api/categories/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categorie = JsonConvert.DeserializeObject<Categorie>(apiResponse);
                }
            }
            return View(categorie);
        }


        
        [HttpPost]
        public async Task<IActionResult> Edit(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    //  Categorie theCategorie = null;
                    using (var response = await httpClient.PutAsJsonAsync("http://localhost:53939/api/categories/" + categorie.Id.ToString(), categorie))
                    {
                        response.EnsureSuccessStatusCode();
                        //  theCategorie = await response.Content.ReadAsAsync<Categorie>();
                    }

                    return RedirectToAction("Index");
                }
            }

            return View(categorie);   
        }

        public async Task<HttpStatusCode> Delete(int Id)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync("http://localhost:53939/api/categories/" + Id.ToString());

                // return RedirectToAction("Index");
                return response.StatusCode;
            }
        }
    }
}