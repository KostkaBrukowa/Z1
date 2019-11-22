using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Z01.Controllers;
using Z01.Exceptions;
using Z01.Models;

namespace Z01.services
{
  public class NoteService
  {
    private readonly MyContext _myContext;

    public NoteService(MyContext myContext)
    {
      _myContext = myContext;
    }

    private void UpdateCategories(Note noteToUpdate, string[] categories)
    {
      foreach (var categoryName in categories)
      {
        if (noteToUpdate.NoteCategories.Any(it => it.Category.Name == categoryName)) continue;

        var category = _myContext.Categories.SingleOrDefault(t => t.Name == categoryName);

        if (category == null)
        {
          category = new Category { Name = categoryName };
          _myContext.Categories.Add(category);
        }

        noteToUpdate.NoteCategories.Add(new NoteCategory()
        {
          Category = category,
          Note = noteToUpdate,
          CategoryID = category.CategoryID,
          NoteID = noteToUpdate.NoteID
        });
      }

      var categoriesToDelete = noteToUpdate.NoteCategories
          .Where(noteCategory => categories.All(it => it != noteCategory.Category.Name))
          .ToList();

      categoriesToDelete.ForEach(it => noteToUpdate.NoteCategories.Remove(it));
    }

    private async Task<bool> UpdateNote(Note note, string[] categories)
    {
      var noteToUpdate = _myContext.Notes
          .Include(it => it.NoteCategories)
          .ThenInclude(it => it.Category)
          .FirstOrDefault(it => it.NoteID == note.NoteID);

      if (noteToUpdate == null)
      {
        throw new EntityNotFoundException();
      }

      _myContext.Entry(noteToUpdate).Property("RowVersion").OriginalValue = note.RowVersion;

      noteToUpdate.Title = note.Title;
      noteToUpdate.Description = note.Description;
      noteToUpdate.Markdown = note.Markdown;

      UpdateCategories(noteToUpdate, categories);

      await _myContext.SaveChangesAsync();

      return true;
    }

    private async Task<bool> SaveNewNote(Note note, string[] categories)
    {
      note.NoteCategories = new List<NoteCategory>();
      _myContext.Notes.Add(note);

      UpdateCategories(note, categories);

      await _myContext.SaveChangesAsync();

      return true;
    }

    public Tuple<int, int, List<Note>> GetAllNotes(NoteFilterModel filterModel)
    {
      var notes = _myContext.Notes
          .Include(it => it.NoteCategories)
          .ThenInclude(it => it.Category);

      var filteredNotes = notes
          .Where(it =>
              filterModel.SelectedCategory == null ||
              it.NoteCategories.FirstOrDefault(_ => _.Category.Name == filterModel.SelectedCategory) != null)
          .Where(it => it.NoteDate >= filterModel.From)
          .Where(it => it.NoteDate <= filterModel.To)
          .ToList();

      var maxPages = (int)Math.Ceiling((double)((filteredNotes.Count - 1) / (HomeController.PAGE_SIZE)));
      filterModel.TrimPages(maxPages);

      var paginatedNotes = filteredNotes
          .Skip(filterModel.Page * HomeController.PAGE_SIZE)
          .Take(HomeController.PAGE_SIZE)
          .ToList();

      return new Tuple<int, int, List<Note>>(maxPages, filteredNotes.Count, paginatedNotes);
    }

    public Note GetNoteById(int id)
    {
      var todoItem = _myContext.Notes
          .Include(it => it.NoteCategories)
          .ThenInclude(it => it.Category);

      return todoItem.FirstOrDefault(it => it.NoteID == id);
    }

    public Task<bool> SaveNote(Note note, string[] categories)
    {
      return note.NoteID == 0 ? SaveNewNote(note, categories) : UpdateNote(note, categories);
    }

    public async Task<bool> RemoveNote(int id)
    {
      var todoItem = await _myContext.Notes.FindAsync(id);

      if (todoItem == null)
      {
        throw new EntityNotFoundException();
      }

      _myContext.Notes.Remove(todoItem);
      await _myContext.SaveChangesAsync();

      return true;
    }

    public HashSet<Category> GetAllCategories()
    {
      var noteCategories = _myContext.NoteCategories
          .Include(it => it.Category)
          .Include(it => it.Note)
          .Where(it => it.Note != null)
          .Select(it => it.Category)
          .ToHashSet();

      return noteCategories;
    }
  }
}
