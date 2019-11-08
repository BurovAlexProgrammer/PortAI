using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RaxhetMvc.Models;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RaxhetMvc.Controllers
{
    public class DBAccessController : Controller
    {
        //DBAccess testDB = new DBAccess();
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind] ErrorViewModel errorViewModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    //string r = testDB.AddRecord(errorViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View();
        }
    }
}
