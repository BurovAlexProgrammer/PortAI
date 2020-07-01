using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BurovavMvcPort.Models;
using BurovavMvcPort.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BurovavMvcPort.Controllers
{
    public class RestAPIController : Controller
    {
        public string apiKey = "fe5014b6b8fbc18a3b524c96eea4e3ee";

        public IActionResult Index()
        {
            return View();

            
        }

    
        public IActionResult Home()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Home(string searchText, bool showAdvanceSearchToggle, string exampleSelect1, int yearBegin, int yearEnd, string exampleSelect2)
        //{
        //    if (searchText == null)
        //    {
        //        //Don't search
        //    }
        //    return View();
        //}

        public JsonResult GetFilmCount(string val)
        {
            return Json("Good!! val:"+val);
        }

        public JsonResult StarSearch(string query, int? year)
        {
            var currLanguage = new LanguageService().GetCurrentLanguage(HttpContext);
            var url = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&language={currLanguage}&query={query}";
            if (year != null)
                url += $"&year={year}";
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
                JObject result = JObject.Parse(response);
                var total_results = result.SelectToken("total_results");
                var total_pages = result.SelectToken("total_pages");


                return Json(new { total_results = total_results, total_pages = total_pages });


            }
        }


        //Получить список жанров
        public JsonResult GetGenres()
        {
            var currLanguage = new LanguageService().GetCurrentLanguage(HttpContext);
            var url = $"https://api.themoviedb.org/3/genre/movie/list?api_key={apiKey}&language={currLanguage}";

            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
                JObject result = JObject.Parse(response);
                var genres = result.SelectToken("genres").Select(g => g.ToObject<Genre>()).ToList();

                return Json(genres);
            }
        }


        /*
        Movie movie = Newtonsoft.Json.JsonConvert.DeserializeObject<Movie>(response);

        if (movie.Response)
        {
            fName.Content = movie.Title;
            fYear.Content = movie.Year;
            fActors.Content = movie.Actors;
            fCountry.Content = movie.Country;
            fLang.Content = movie.Language;
            fGenre.Content = movie.Genre;
            fDesc.Text = movie.Plot;

            if (movie.Poster != "N/A")
            {
                Thread t = new Thread(() => LoadImage(movie));
                t.Start();
            }
            else
                fPoster.Source = null;
        }
        else
        {
            MessageBox.Show(movie.Error, "Фильм не найден", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        */

    }
}