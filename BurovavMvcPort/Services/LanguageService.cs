using BurovavMvcPort.Data.RestAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BurovavMvcPort.Services
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILanguageService languageService)
        {
            languageService.InitLanguage(context);
            await _next.Invoke(context);
        }
    }

    public class LanguageService : ILanguageService
    {
        public static readonly List<Language> Languages =
            new List<Language> {
                new Language(1, "En", "English", "en-EN"),
                new Language(2, "Ru", "Русский", "ru-RU"),
                new Language(3, "De", "Deutsche", "de-DE"),
                new Language(4, "Fi", "Suomalainen", "fi-FI")
            };
        public static readonly Language defaultLanguage = Languages.First();
        Language currentLanguage;

        public string GetCurrentLanguage(HttpContext context)
        {

            if (context.Request.Cookies.ContainsKey(CookieKeys.LANGUAGE))
            {
                var languageCookie = context.Request.Cookies[CookieKeys.LANGUAGE];

                if (Languages.Where(l => l.culture == languageCookie).Any())
                    return languageCookie;
                else 
                    return defaultLanguage.culture;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Записать текущий язык в куки и вернуть успешность выполнения.
        /// </summary>
        public bool SetCurrentLanguage(HttpContext context, string lang)
        {
            if (Languages.Any(l => l.culture == lang))
            {
                context.Response.Cookies.Append(CookieKeys.LANGUAGE, lang);
                currentLanguage = Languages.First(l => l.culture == lang);
                SetUI(context);
                return true;
            }
            else return false;

        }

        public void InitLanguage(HttpContext context)
        {
            var cookieLanguage = GetCurrentLanguage(context);
            if  (cookieLanguage == null) 
                SetCurrentLanguage(context, Languages.First().culture);
            else
                SetCurrentLanguage(context, cookieLanguage);
            SetUI(context);
        }

        public void SetUI(HttpContext context)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(currentLanguage.culture);
        }

        public Language GetLanguageByName(string name)
        {
            return Languages.DefaultIfEmpty(null).
                FirstOrDefault(l => l.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public class Language : ILanguage
    {
        public Language(int order, string name, string title, string culture)
        {
            this.order = order;
            this.name = name;
            this.title = title;
            this.culture = culture;
        }
        public int order { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string culture { get; set; }
    }
    public interface ILanguageService
    {
        void SetUI(HttpContext context);
        void InitLanguage(HttpContext context);
        string GetCurrentLanguage(HttpContext context);
        bool SetCurrentLanguage(HttpContext context, string lang);
    }

    public interface ILanguage
    {
        int order { get; set; }
        string name { get; set; }
        string title { get; set; }
        string culture { get; set; }
    }
}
