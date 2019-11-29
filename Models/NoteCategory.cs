using System.ComponentModel.DataAnnotations.Schema;

namespace Z01.Models
{
  [Table("NoteCategory")]
  public class NoteCategory
  {
    public int NoteID { get; set; }
    public Note Note { get; set; }

    public int CategoryID { get; set; }
    public Category Category { get; set; }
  }
}
