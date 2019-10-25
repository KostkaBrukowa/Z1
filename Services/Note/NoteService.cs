using System;
using System.Collections.Generic;
using System.Linq;
using Z01.Controllers;
using Z01.Exceptions;
using Z01.Models;
using Z01.Repositories;

namespace Z01.services
{
    public class NoteService
    {
        private readonly NoteRepository _noteRepository = new NoteRepository();

        private static bool IsTitleUnique(NoteModel note, List<NoteModel> notes)
        {
            return !notes.Exists(it =>
                string.Equals(it.Title, note.Title, StringComparison.CurrentCultureIgnoreCase) && it.Id != note.Id);
        }

        private void UpdateNote(NoteModel note, List<NoteModel> notes)
        {
            if (!notes.Exists(it => it.Id == note.Id))
            {
                throw new EntityNotFoundException();
            }

            if (!IsTitleUnique(note, notes))
            {
                throw new TitleNotUniqueException();
            }

            _noteRepository.RemoveNote(note.Id);
            _noteRepository.SaveNoteToDatabase(note);
        }

        private void SaveNewNote(NoteModel note, List<NoteModel> notes)
        {
            if (!IsTitleUnique(note, notes))
            {
                throw new TitleNotUniqueException();
            }

            _noteRepository.SaveNoteToDatabase(new NoteModel(note, Guid.NewGuid().ToString()));
        }

        public Tuple<int, List<NoteModel>> GetAllNotes(NoteFilterModel filterModel)
        {
            var notes = _noteRepository.LoadNotes();
            var filteredNotes = notes
                .Where(it =>
                    filterModel.SelectedCategory == null || it.Categories.Contains(filterModel.SelectedCategory))
                .Where(it => it.CreationDate >= filterModel.From)
                .Where(it => it.CreationDate <= filterModel.To)
                .ToList();
            var paginatedNotes = filteredNotes
                .Skip(filterModel.Page * HomeController.PAGE_SIZE)
                .Take(HomeController.PAGE_SIZE)
                .ToList();

            return new Tuple<int, List<NoteModel>>(filteredNotes.Count, paginatedNotes);
        }

        public NoteModel GetNoteById(string id)
        {
            var notes = _noteRepository.LoadNotes();

            return notes.Find(it => it.Id == id);
        }

        public void SaveNote(NoteModel note)
        {
            var notes = _noteRepository.LoadNotes();

            if (note.Id == null)
                SaveNewNote(note, notes);
            else
                UpdateNote(note, notes);
        }

        public void RemoveNote(string id)
        {
            var removeSucceed = _noteRepository.RemoveNote(id);

            if (!removeSucceed)
            {
                throw new EntityNotFoundException();
            }
        }
    }
}
