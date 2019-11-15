using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Z01.Models;
using Z01.services;

namespace Z01.Controllers
{
    public class HomeController : Controller
    {
//        private readonly CategoryService _categoryService = new CategoryService();
        private readonly NoteService _noteService;
        private readonly MyContext _myContext;
        public static readonly int PAGE_SIZE = 3;

        public HomeController(MyContext myContext)
        {
            _myContext = myContext;
            _noteService = new NoteService(myContext);
        }

        public IActionResult Index(NoteFilterModel filters)
        {
            // if (!_myContext.Notes.Any())
            // {
            //     // Create a new TodoItem if collection is empty,
            //     // which means you can't delete all TodoItems.
            //     _myContext.Notes.Add(new Note {Markdown = true, Content = "test content", Title = "title"});
            //     _myContext.SaveChanges();
            // }

            var (maxPages, allNotesCount, notes) = _noteService.GetAllNotes(filters);
            var categories = _noteService.GetAllCategories();

            return View(new IndexViewModel(filters, categories, notes, maxPages));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
