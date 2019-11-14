using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Z01.Models
{
    public class IndexViewModel
    {
        public IndexViewModel() : this(new NoteFilterModel(), new List<Category>(), new List<Note>(), 0)
        {
        }

        public IndexViewModel(NoteFilterModel filters, IEnumerable<Category> categories, List<Note> notes,
            int maxPage)
        {
            Filters = filters.TrimPages(maxPage);
            Notes = notes;
            MaxPage = maxPage;
            Categories = new List<SelectListItem>();

            foreach (var category in categories)
            {
                Categories.Add(new SelectListItem {Value = category.Name, Text = category.Name});
            }
        }

        public List<SelectListItem> Categories { get; set; }

        public NoteFilterModel Filters { get; set; }

        public List<Note> Notes { get; set; }

        public int MaxPage { get; set; }
    }
}
