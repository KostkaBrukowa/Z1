using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z01.Models
{
  [Table("Category")]
  public class Category
  {
    public int CategoryID { get; set; }
    [Required]
    [MinLength(1), MaxLength(64)]
    public string Name { get; set; }

    public List<NoteCategory> NoteCategories { get; set; }
  }
}
