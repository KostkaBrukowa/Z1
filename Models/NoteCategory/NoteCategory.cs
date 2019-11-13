using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z01.Models.Category
{
    public class NoteCategory
    {
        [ForeignKey(nameof(NoteModel))] 
        public string NoteId { get; set; }
        public NoteModel Note { get; set; }
        [ForeignKey(nameof(CategoryModel))] 
        public string CategoryId { get; set; }
        public CategoryModel Category { get; set; }
    }
}
