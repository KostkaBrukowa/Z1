using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Z01.Models;

namespace Z01.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(string[] categories)
        {
            Console.WriteLine($"!!!!!!!!!!!!!!!{categories.Length}");
            Console.WriteLine($"!!!!!!!!!!!!!!!{categories[0]}");
            return Redirect("/");
        }
    }
}
