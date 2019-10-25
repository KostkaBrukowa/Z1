using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z01.Models;

namespace Z01.Repositories
{
    public class CategoryRepository
    {
        public List<string> GetAllCategories()
        {
            var allFiles = Directory.EnumerateFiles(NoteRepository._path, "*.note.*");
            var allCategories = new HashSet<string>();
            foreach (var file in allFiles)
            {
                var note = NoteModel.FromString(File.ReadAllText(file), file);

                allCategories.UnionWith(note.Categories.Select(c => c.ToLower()));
            }

            return allCategories.ToList();
        }
    }
}
