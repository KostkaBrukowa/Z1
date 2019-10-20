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
        public IActionResult Update()
        {
            return Redirect("/");
        }
    }
}
