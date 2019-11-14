using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Z01.Exceptions;
using Z01.Models;
using Z01.services;

namespace Z01.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteService _noteService;

        public NoteController(MyContext myContext)
        {
            _noteService = new NoteService(myContext);
        }

        public IActionResult Index(int id)
        {
            try
            {
                var note = _noteService.GetNoteById(id);
                return View(new NoteViewModel(note ?? new Note()));
            }
            catch (ArgumentNullException)
            {
                return View(new NoteViewModel(new Note()));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(Note note, string[] categories)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new NoteViewModel(note));
            }

            try
            {
                await _noteService.SaveNote(note, categories);

                return Redirect("/");
            }
            catch (TitleNotUniqueException)
            {
                ModelState.AddModelError("Title", "Title is not unique");

                return View("Index", new NoteViewModel(note));
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(400);
            }
        }

        public async Task<IActionResult> Remove(int id, NoteFilterModel filters)
        {
            try
            {
                await _noteService.RemoveNote(id);

                return RedirectToAction("Index", "Home", filters);
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(400);
            }
        }
    }
}
