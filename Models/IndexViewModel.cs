using System;
using System.Collections.Generic;

namespace Z01.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            From = DateTime.Today.AddDays(-1);
            To = DateTime.Today;
            Categories = new List<string>(new string[] {"jarek", "dominik"});
            Notes = new List<NoteModel>(new NoteModel[]
            {
                new NoteModel(),
            });
            Page = 3;
        }

        public IndexViewModel(DateTime from, DateTime to, List<string> categories, List<NoteModel> notes, int page,
            string selectedCategory)
        {
            From = from;
            To = to;
            Categories = categories;
            Notes = notes;
            Page = page;
            SelectedCategory = selectedCategory;
        }

        public string Id { get; }
        public DateTime From { get; }
        public DateTime To { get; }
        public List<string> Categories { get; }
        public List<NoteModel> Notes { get; }
        public int Page { get; }
        public string SelectedCategory { get; }
    }
}
