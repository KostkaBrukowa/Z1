using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Z01.Models.Category
{
    public class CategoryModel
    {
        public CategoryModel()
        {
        }

        public CategoryModel(string name)
        {
            Name = name;
        }

        public string Id { get; set; }
        [StringLength(60, MinimumLength = 3)] public string Name { get; set; }
        public ICollection<NoteCategory> NoteCategories { get; set; }
    }
}
