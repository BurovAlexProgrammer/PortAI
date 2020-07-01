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
        //Уточнить, возможно есть класс для возвращения правильного кода выполнения запроса
        [Route("app/SetLanguage/{name}")]
        public IActionResult SetLanguage(string name)
        {
            new LanguageService().SetCurrentLanguage(HttpContext, name);
            if (name == null)
                return new ObjectResult(false);

            return new ObjectResult(true);
        }
    }
}
