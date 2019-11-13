using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Z01.Controllers;
using Z01.Exceptions;
using Z01.Models;
using Z01.Models.Category;

namespace Z01.services
{
    public class NoteService
    {
//        private readonly NoteRepository _noteRepository = new NoteRepository();
        private readonly NoteContext _noteContext;
        private readonly CategoryContext _categoryContext;
        private readonly NoteCategoryContext _noteCategoryContext;

        public NoteService(NoteContext noteContext, CategoryContext categoryContext,
            NoteCategoryContext noteCategoryContext)
        {
            _noteContext = noteContext;
            _categoryContext = categoryContext;
            _noteCategoryContext = noteCategoryContext;
        }

        private bool IsTitleUnique(NoteModel noteModel)
        {
            var note = _noteContext.NoteItems.FirstOrDefault((it) => it.Title == noteModel.Title);

            return note == null || noteModel.Id == note.Id;
        }

        private async Task<bool> UpdateNote(NoteModel note, string[] categories)
        {
            if (!IsTitleUnique(note))
            {
                throw new TitleNotUniqueException();
            }

            var noteToUpdate = _noteContext.NoteItems.FirstOrDefault(it => it.Id == note.Id);

            if (note == null)
            {
                throw new EntityNotFoundException();
            }

//            _noteContext.Update(noteModel);
            noteToUpdate.Title = note.Title;
            noteToUpdate.Content = note.Content;
            noteToUpdate.Markdown = note.Markdown;

            foreach (var categoryName in categories)
            {
                var category = _categoryContext.CategoryItems.SingleOrDefault(t => t.Name == categoryName);

                if (category == null)
                {
                    category = new CategoryModel(categoryName);
                    _categoryContext.CategoryItems.Add(category);
                }

                _noteCategoryContext.NoteCategories.Add(new NoteCategory()
                    {Category = category, Note = noteToUpdate, CategoryId = category.Id, NoteId = noteToUpdate.Id});
            }

            await _noteCategoryContext.SaveChangesAsync();
            await _categoryContext.SaveChangesAsync();
            await _noteContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SaveNewNote(NoteModel note, string[] categories)
        {
            if (!IsTitleUnique(note))
            {
                throw new TitleNotUniqueException();
            }

            _noteContext.NoteItems.Add(note);
            await _noteContext.SaveChangesAsync();

            return true;
        }

        public Tuple<int, List<NoteModel>> GetAllNotes(NoteFilterModel filterModel)
        {
            var notes = _noteContext.NoteItems;

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

            return new Tuple<int, List<NoteModel>>(filteredNotes.Count, paginatedNotes);
        }

        public async Task<NoteModel> GetNoteById(string id)
        {
            var todoItem = await _noteContext.NoteItems.FindAsync(id);

            return todoItem;
        }

        public Task<bool> SaveNote(NoteModel note, string[] categories)
        {
            return note.Id == null ? SaveNewNote(note, categories) : UpdateNote(note, categories);
        }

        public async Task<bool> RemoveNote(string id)
        {
            var todoItem = await _noteContext.NoteItems.FindAsync(id);

            if (todoItem == null)
            {
                throw new EntityNotFoundException();
            }

            _noteContext.NoteItems.Remove(todoItem);
            await _noteContext.SaveChangesAsync();

            return true;
        }

        public HashSet<CategoryModel> GetAllCategories()
        {
            var categories = _categoryContext.CategoryItems.ToHashSet();

            return categories;
        }
    }
}
