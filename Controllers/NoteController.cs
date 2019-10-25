using Microsoft.AspNetCore.Mvc;
using Z01.Exceptions;
using Z01.Models;
using Z01.services;

namespace Z01.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteService _noteService = new NoteService();

        public IActionResult Index(string id)
        {
            var note = _noteService.GetNoteById(id);

            return note == null ? View(new NoteModel()) : View(note);
        }

        [HttpPost]
        public IActionResult Save(NoteModel note)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", note);
            }

            try
            {
                _noteService.SaveNote(note);

                return Redirect("/");
            }
            catch (TitleNotUniqueException)
            {
                ModelState.AddModelError("Title", "Title is not unique");

                return View("Index", note);
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(400);
            }
        }

        public IActionResult Remove(string id)
        {
            try
            {
                _noteService.RemoveNote(id);

                return Redirect("/");
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(400);
            }
        }
    }
}
