using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BurovavMvcPort.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BurovavMvcPort.Controllers
{
    public class RestAPIController : Controller
    {
        public string apiKey = "fe5014b6b8fbc18a3b524c96eea4e3ee";
        public string currLanguage = "ru-RU";

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

        //Получить список жанров
        public JsonResult GetGenres()
        {
            var url = $"https://api.themoviedb.org/3/genre/movie/list?api_key={apiKey}&language={currLanguage}";

            

            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
                JObject result = JObject.Parse(response);
                var genres = result.SelectToken("genres").Select(g => g.ToObject<Genre>()).ToList();


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
                return Json(genres);
            }

            return Json(null);
        }


    }
}