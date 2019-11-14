//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//
//namespace Z01.Models
//{
//    public class NoteModel
//    {
//        public NoteModel()
//        {
//            Id = null;
//            CreationDate = DateTime.Now;
//            Title = "";
//            Content = "";
//            Markdown = false;
//            NoteCategories = new List<NoteCategory>();
//        }
//
//        public NoteModel(NoteModel note)
//        {
//            CreationDate = DateTime.Now;
//            Title = note.Title;
//            Markdown = note.Markdown;
//            Content = note.Content;
//        }
//
//        public NoteModel(NoteModel note, string id) : this(note)
//        {
//            Id = id;
//        }
//
//        public NoteModel(string id, DateTime creationDate, string title, bool markdown, string content)
//        {
//            Id = id;
//            CreationDate = creationDate;
//            Title = title;
//            Markdown = markdown;
//            Content = content;
//        }
//
//        public string Id { get; set; }
//
//        public DateTime CreationDate { get; set; }
//
//        [Required]
////        [Index(IsUnique=true)]
//        [StringLength(60, MinimumLength = 3)]
//        public string Title { get; set; }
//
//        public bool Markdown { get; set; }
//
//        [Required]
//        [StringLength(1060, MinimumLength = 3)]
//        public string Content { get; set; }
//
//        public ICollection<NoteCategory> NoteCategories { get; set; }
////
////        public override string ToString()
////        {
////            var categories = String.Join(", ", Categories);
////            var date = $"{CreationDate:yyyy-MM-dd}";
////
////            return $"id: {Id}\n" +
////                   $"categories: {categories}\n" +
////                   $"date: {date}\n" +
////                   $"title: {Title}\n" +
////                   $"{Content}";
////        }
////
////        private static string GetStringValue(string line)
////        {
////            var tokens = line.Split(": ");
////
////            return string.Join(": ", tokens.Skip(1));
////        }
////
////        public static NoteModel FromString(string noteDTO, string filename)
////        {
////            var lines = noteDTO.Split('\n');
////
////            var id = GetStringValue(lines[0]);
////            var categories = GetStringValue(lines[1]).Split(", ");
////            var date = GetStringValue(lines[2]);
////            var markdown = filename.EndsWith("md");
////            var title = GetStringValue(lines[3]);
////            var content = string.Join("\n", lines.Skip(4));
////
////            return new NoteModel(id, DateTime.Parse(date), title, markdown, content, categories);
////        }
//    }
//}
