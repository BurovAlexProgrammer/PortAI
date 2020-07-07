using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortAI.Services;

namespace PortAI.Controllers
{
    public class BurovavController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Portfolio()
        {
            return View();
        }

        [Route("BurovAV/SetLanguage/{culture}")]
        public IActionResult SetLanguage(string culture)
        {
            new LanguageService().SetCurrentLanguage(HttpContext, culture);
            if (culture == null)
                return new ObjectResult(false);
            return new ObjectResult(true);
        }
    }
}