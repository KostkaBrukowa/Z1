using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Z01.Models;
using Z01.services;

namespace Z01.Controllers
{
    public class HomeController : Controller
    {
        private readonly CategoryService _categoryService = new CategoryService();
        private readonly NoteService _noteService = new NoteService();
        public static int PAGE_SIZE = 5;

        public IActionResult Index(NoteFilterModel filters)
        {
            var (allNotesCount, notes) = _noteService.GetAllNotes(filters);
            var categories = _categoryService.GetAllCategories();
            var maxPages = Math.Ceiling((double) ((allNotesCount - 1) / (PAGE_SIZE)));

            return View(
                new IndexViewModel(filters, categories, notes, (int)maxPages
            ));
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
