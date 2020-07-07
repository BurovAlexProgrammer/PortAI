using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BurovavMvcPort.Models;
using BurovavMvcPort.Models.RestAPI;
using BurovavMvcPort.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BurovavMvcPort.Controllers
{
    public class RestAPIController : Controller
    {
        public string apiKey = "fe5014b6b8fbc18a3b524c96eea4e3ee";

        /// <summary>
        /// Страница с поиском фильмов
        /// </summary>
        /// <returns></returns>
        public IActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// Отображение всей информации о фильме. 
        /// </summary>
        /// <param name="id">ID фильма</param>
        /// <returns></returns>
        public IActionResult Film(long? id)
        {
            var currLanguage = new LanguageService().GetCurrentLanguage(HttpContext);
            var url = $"https://api.themoviedb.org/3/movie/{id}?api_key={apiKey}&language={currLanguage}";
            JObject jsonAnswer;
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
                jsonAnswer = JObject.Parse(response);
            }
            ViewBag.Film = jsonAnswer;
            return View();
        }

        /// <summary>
        /// Поиск фильмов по параметрам. Возвращает List of Film
        /// </summary>
        /// <param name="query">Искомый текст (обязательно)</param>
        /// <param name="year">Год релиза</param>
        /// <param name="sort">Тип сортировки</param>
        /// <param name="genreId">Id жанра</param>
        /// <param name="vote">Минимальная оценка</param>
        /// <returns></returns>
        public JsonResult StarSearch(string query, int? year, string sort, int? genreId, string vote)
        {
            JObject jsonAnswer;
            var total_results = 0;
            var total_pages = 0;
            var currLanguage = new LanguageService().GetCurrentLanguage(HttpContext);
            var url = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&language={currLanguage}&query={query}";
            if (year != null)
                url += $"&year={year}";
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
                jsonAnswer = JObject.Parse(response);
                total_results = jsonAnswer.SelectToken("total_results")?.ToObject<int>() ?? 0;
                total_pages = jsonAnswer.SelectToken("total_pages")?.ToObject<int>() ?? 0;
            }

            List<Film> result = new List<Film>();
            //Асинхронная загрузка всех страниц json
            Task[] pageTasks = new Task[total_pages];
            for (int i = 0; i < pageTasks.Length; i++)
            {
                var pageUrl = url + "&page="+(i+1);
                pageTasks[i] = Task.Factory.StartNew(() =>
                {
                    using (var webClient = new WebClient())
                    {
                        var response = webClient.DownloadString(pageUrl);
                        jsonAnswer = JObject.Parse(response);
                        //var j1 = jsonAnswer["results"];
                        var r1 = jsonAnswer.SelectToken("results").First;
                        var results = jsonAnswer.SelectToken("results")?.Select(r => r.ToObject<Film>())?.ToList();
                        lock (result)
                        {
                            foreach (var item in results)
                                result.Add(item);
                        }
                    }
                });
            }
            Task.WaitAll(pageTasks);
            //Обработка результатов
            if (year != null)
            {
                result = result.Where(r => r.year == year).ToList();
            }
            switch (sort)
            {
                case "popularDesc":
                    result = result.OrderByDescending(f => f.popularity).ToList();
                    break;
                case "popularAsc":
                    result = result.OrderBy(f => f.popularity).ToList();
                    break;
                case "dateDesc":
                    result = result.OrderByDescending(f => f.release_date).ToList();
                    break;
                case "dateAsc":
                    result = result.OrderBy(f => f.release_date).ToList();
                    break;
                case "nameDesc":
                    result = result.OrderByDescending(f => f.title).ToList();
                    break;
                case "nameAsc":
                    result = result.OrderBy(f => f.title).ToList();
                    break;
            }
            if (genreId != null & genreId != 0)
            {
                result = result.Where(r => r.genre_ids.Contains(genreId ?? 0)).ToList();
            }
            if (vote != Resources.Resource.Any)
            {
                result = result.Where(r => r.vote_average >= int.Parse(vote)).ToList();
            }
            return Json(result);
        }

        /// <summary>
        /// Получение списока жанров
        /// </summary>
        /// <returns>List of Genre</returns>
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
    }
}