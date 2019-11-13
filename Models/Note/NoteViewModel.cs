using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Z01.Models.Category;

namespace Z01.Models
{
    public class NoteViewModel
    {
        public NoteViewModel(NoteModel note)
        {
            Id = note.Id;
            CreationDate = note.CreationDate;
            Title = note.Title;
            Content = note.Content;
            Markdown = note.Markdown;
            Categories = new HashSet<CategoryModel>();

            foreach (var noteCategory in note.NoteCategories)
            {
                if (noteCategory.NoteId == note.Id)
                {
                    Categories.Add(noteCategory.Category);
                }
            }
        }

        public string Id { get; set; }
        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        public bool Markdown { get; set; }

        [Required]
        [StringLength(1060, MinimumLength = 3)]
        public string Content { get; set; }

        public ICollection<CategoryModel> Categories { get; set; }
//
//        public override string ToString()
//        {
//            var categories = String.Join(", ", Categories);
//            var date = $"{CreationDate:yyyy-MM-dd}";
//
//            return $"id: {Id}\n" +
//                   $"categories: {categories}\n" +
//                   $"date: {date}\n" +
//                   $"title: {Title}\n" +
//                   $"{Content}";
//        }
//
//        private static string GetStringValue(string line)
//        {
//            var tokens = line.Split(": ");
//
//            return string.Join(": ", tokens.Skip(1));
//        }
//
//        public static NoteModel FromString(string noteDTO, string filename)
//        {
//            var lines = noteDTO.Split('\n');
//
//            var id = GetStringValue(lines[0]);
//            var categories = GetStringValue(lines[1]).Split(", ");
//            var date = GetStringValue(lines[2]);
//            var markdown = filename.EndsWith("md");
//            var title = GetStringValue(lines[3]);
//            var content = string.Join("\n", lines.Skip(4));
//
//            return new NoteModel(id, DateTime.Parse(date), title, markdown, content, categories);
//        }
    }
}
