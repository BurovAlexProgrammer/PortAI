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
                new Language(2, "Be", "Беларускі", "be-BE"),
                new Language(3, "Cs", "Čeština", "cs-CS"),
                new Language(4, "De", "Deutsche", "de-DE"),
                new Language(5, "Fi", "Suomalainen", "fi-FI"),
                new Language(6, "Fr", "Français", "fr-FR"),
                new Language(7, "Ru", "Русский", "ru-RU"),
                new Language(8, "Sv", "Svenska", "sv-SV"),
            };
        public static readonly Language defaultLanguage = Languages.First();
        Language currentLanguage;

        /// <summary>
        /// Получение текущего языка из куки. Возвращает строку идентификатора культуры (en-EN)
        /// </summary>
        /// <param name="context">Текущий контекст</param>
        /// <returns></returns>
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

        /// <summary>
        /// Инициализация. Проверяет есть ли в куки указания на выбранный язык. Если нету, то записывает в куки язык по умолчанию
        /// </summary>
        /// <param name="context"></param>
        public void InitLanguage(HttpContext context)
        {
            var cookieLanguage = GetCurrentLanguage(context);
            if  (cookieLanguage == null) 
                SetCurrentLanguage(context, defaultLanguage.culture);
            else
                SetCurrentLanguage(context, cookieLanguage);
            SetUI(context);
        }

        /// <summary>
        /// Применяет культуру к текущему контексту
        /// </summary>
        /// <param name="context"></param>
        public void SetUI(HttpContext context)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(currentLanguage.culture);
        }

        /// <summary>
        /// Возвращает экземпляр класса Language по имени
        /// </summary>
        /// <param name="culture">Строка иднтификатор культуры (например 'en-EN')</param>
        /// <returns></returns>
        public Language GetLanguageByCulture(string culture)
        {
            return Languages.DefaultIfEmpty(null).
                FirstOrDefault(l => l.name.Equals(culture, StringComparison.InvariantCultureIgnoreCase));
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
