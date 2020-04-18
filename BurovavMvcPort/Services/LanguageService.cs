using BurovavMvcPort.Data.RestAPI;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
            languageService.InitCurrentLanguage(context);
            await _next.Invoke(context);
        }
    }

    public class LanguageService : ILanguageService
    {
        public static readonly Dictionary<string, string> Languages = 
            new Dictionary<string, string> {["en"] = "English", ["ru"] = "Русский"};
        string currentLanguage;

        public LanguageService()
        {
            currentLanguage = Languages.First().Key;
        }

        /// <summary>
        /// Считать текущий язык из куки
        /// </summary>
        public string GetCurrentLanguage(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey(CookieKeys.LANGUAGE))
            {
                return context.Request.Cookies[CookieKeys.LANGUAGE];
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Записать текущий язык в куки и вернуть успешность выполнения.
        /// </summary>
        public bool SetCurrentLanguage(HttpContext context, string lang)
        {
            if (Languages.ContainsKey(lang))
            {
                context.Response.Cookies.Append(CookieKeys.LANGUAGE, lang);
                return true;
            }
            else return false;
        }

        public void InitCurrentLanguage(HttpContext context)
        {
            if (GetCurrentLanguage(context) == null)
                SetCurrentLanguage(context, currentLanguage);
        }
    }


    public interface ILanguageService
    {
        void InitCurrentLanguage(HttpContext context);
        string GetCurrentLanguage(HttpContext context);
        bool SetCurrentLanguage(HttpContext context, string lang);

    }
}
