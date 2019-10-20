using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Z01.Models;

namespace Z01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(DateTime from, DateTime to, string category, int page = 1)
        {
            var categories = new List<string>(new string[] {"category 1", "category2"});
            if (from == new DateTime())
            {
                from = DateTime.Now.AddDays(-7);
            }

            if (to == new DateTime())
            {
                to = DateTime.Now;
            }

            if (page <= 0)
            {
                page = 1;
            }

            return View(new IndexViewModel(from, to, categories, new List<NoteModel>(), page, category));
        }

        [HttpGet]
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
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
