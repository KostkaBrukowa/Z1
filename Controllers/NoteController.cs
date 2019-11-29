using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
      catch (DbUpdateConcurrencyException)
      {
        ModelState.AddModelError("Title", "Another user has updated note before you");

        return View("Index", new NoteViewModel(note));
      }
      catch (SqlException)
      {
        ModelState.AddModelError("Title", "There is a note with the same title. Please change the title.");

        return View("Index", new NoteViewModel(note));
      }
      catch (DbUpdateException)
      {
        ModelState.AddModelError("Title", "There is a note with the same title. Please change the title.");

        return View("Index", new NoteViewModel(note));
      }
      catch (EntityNotFoundException)
      {
        return RedirectToAction("Index", "Home", new NoteFilterModel { Error = "Someone has already deleted this note." });
      }
    }

    public async Task<IActionResult> Remove(int id, byte[] rowVersion, NoteFilterModel filters)
    {
      try
      {
        await _noteService.RemoveNote(id, rowVersion);

        return RedirectToAction("Index", "Home", filters);
      }
      catch (ConcurrencyException)
      {
        filters.Error = "Someone has already updated this note.";

        return RedirectToAction("Index", "Home", filters);
      }
      catch (EntityNotFoundException)
      {
        return StatusCode(400);
      }
    }
  }
}
