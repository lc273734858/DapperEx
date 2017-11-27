using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FWW.Framework.EncryptUI.Models;
using Microsoft.Extensions.Primitives;

namespace FWW.Framework.EncryptUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public string Encrypt(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return "";
            }
            var result=FWW.Framework.EncryptUI.Common.EncryptHelper.EncrypConnectionstring(password);
            return result;
        }
    }
}
