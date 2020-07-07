using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BurovavMvcPort.Services;
using Microsoft.AspNetCore.Mvc;

namespace BurovavMvcPort.Controllers
{
    public class AppController : Controller
    {
        [Route("app/SetLanguage/{culture}")]
        public IActionResult SetLanguage(string culture)
        {
            new LanguageService().SetCurrentLanguage(HttpContext, culture);
            if (culture == null)
                return new ObjectResult(false);

            return new ObjectResult(true);
        }
    }
}
