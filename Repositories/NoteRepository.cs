using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z01.Models;

namespace Z01.Repositories
{
    public class NoteRepository
    {
        private static readonly string _fileName = "/notes/";

        public static readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                              _fileName;

        public List<NoteModel> LoadNotes()
        {
            return Directory.EnumerateFiles(_path, "*.note.*")
                .Select(file => NoteModel.FromString(File.ReadAllText(file), file))
                .ToList();
        }

        public Tuple<int, List<NoteModel>> LoadNotes(int page, int size)
        {
            var allFiles = Directory.EnumerateFiles(_path, "*.note.*");

            return new Tuple<int, List<NoteModel>>(
                allFiles.Count(),
                allFiles.Skip(page * size)
                    .Select(file => NoteModel.FromString(File.ReadAllText(file), file))
                    .Take(size)
                    .ToList());
        }

        public void SaveNoteToDatabase(NoteModel note)
        {
            var extension = note.Markdown ? ".note.md" : ".note.txt";

            File.WriteAllText(_path + note.Title + extension, note.ToString());
        }

        public bool RemoveNote(string id)
        {
            var allFiles = Directory.EnumerateFiles(_path, "*.note.*");
            foreach (var file in allFiles)
            {
                var note = NoteModel.FromString(File.ReadAllText(file), file);

                if (note.Id != id) continue;

                File.Delete(file);
                return true;
            }

            return false;
        }
    }
}
