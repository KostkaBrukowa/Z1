using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Z01.Models
{
    public class IndexViewModel
    {
        public IndexViewModel() : this(new NoteFilterModel(), new List<string>(), new List<NoteModel>(), 0)
        {
        }

        public IndexViewModel(NoteFilterModel filters, IEnumerable<string> categories, List<NoteModel> notes, int maxPage)
        {
            Filters = filters.TrimPages(maxPage);
            Notes = notes;
            MaxPage = maxPage;
            Categories = new List<SelectListItem>();

            foreach (var category in categories)
            {
                Categories.Add(new SelectListItem {Value = category, Text = category});
            }
        }

        public List<SelectListItem> Categories { get; set; }

        public NoteFilterModel Filters { get; set; }

        public List<NoteModel> Notes { get; set; }

        public int MaxPage { get; set; }
    }
}
