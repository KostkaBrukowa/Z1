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

        public NoteController(NoteContext noteContext, CategoryContext categoryContext, NoteCategoryContext noteCategoryContext)
        {
            _noteService = new NoteService(noteContext, categoryContext, noteCategoryContext);
        }

        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var note = await _noteService.GetNoteById(id);
                return View(new NoteViewModel(note));
            }
            catch (ArgumentNullException)
            {
                return View(new NoteViewModel(new NoteModel()));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(NoteModel note, string[] categories)
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

        public async Task<IActionResult> Remove(string id)
        {
            try
            {
                await _noteService.RemoveNote(id);

                return Redirect("/");
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(400);
            }
        }
    }
}
