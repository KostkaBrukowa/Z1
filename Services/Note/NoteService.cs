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

        private bool IsTitleUnique(Note noteModel)
        {
            var note = _myContext.Notes.FirstOrDefault((it) => it.Title == noteModel.Title);

            return note == null || noteModel.NoteID == note.NoteID;
        }

        private void UpdateCategories(Note noteToUpdate, string[] categories)
        {
            foreach (var categoryName in categories)
            {
                if (noteToUpdate.NoteCategories.Any(it => it.Category.Name == categoryName)) continue;

                var category = _myContext.Categories.SingleOrDefault(t => t.Name == categoryName);

                if (category == null)
                {
                    category = new Category {Name = categoryName};
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
        }

        private async Task<bool> UpdateNote(Note note, string[] categories)
        {
            if (!IsTitleUnique(note))
            {
                throw new TitleNotUniqueException();
            }

            var noteToUpdate = _myContext.Notes
                .Include(it => it.NoteCategories)
                .ThenInclude(it => it.Category)
                .FirstOrDefault(it => it.NoteID == note.NoteID);

            if (noteToUpdate == null)
            {
                throw new EntityNotFoundException();
            }

            noteToUpdate.Title = note.Title;
            noteToUpdate.Content = note.Content;
            noteToUpdate.Markdown = note.Markdown;

            UpdateCategories(noteToUpdate, categories);

            await _myContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SaveNewNote(Note note, string[] categories)
        {
            if (!IsTitleUnique(note))
            {
                throw new TitleNotUniqueException();
            }

            note.NoteCategories = new List<NoteCategory>();
            _myContext.Notes.Add(note);

            UpdateCategories(note, categories);

            await _myContext.SaveChangesAsync();

            return true;
        }

        public Tuple<int, List<Note>> GetAllNotes(NoteFilterModel filterModel)
        {
            var notes = _myContext.Notes.Include(it => it.NoteCategories);

            var filteredNotes = notes
//                .Where(it =>
//                    filterModel.SelectedCategory == null ||
//                    it.Categories.First((it1) => it1.Name == filterModel.SelectedCategory) != null)
                .Where(it => it.CreationDate >= filterModel.From)
                .Where(it => it.CreationDate <= filterModel.To)
                .ToList();
            var paginatedNotes = filteredNotes
                .Skip(filterModel.Page * HomeController.PAGE_SIZE)
                .Take(HomeController.PAGE_SIZE)
                .ToList();

            return new Tuple<int, List<Note>>(filteredNotes.Count, paginatedNotes);
        }

        public Note GetNoteById(string id)
        {
            var todoItem = _myContext.Notes
                .Include(it => it.NoteCategories)
                .ThenInclude(it => it.Category);

            return todoItem.FirstOrDefault(it => it.NoteID == id);
        }

        public Task<bool> SaveNote(Note note, string[] categories)
        {
            return note.NoteID == null ? SaveNewNote(note, categories) : UpdateNote(note, categories);
        }

        public async Task<bool> RemoveNote(string id)
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
            var categories = _myContext.Categories.ToHashSet();

            return categories;
        }
    }
}
