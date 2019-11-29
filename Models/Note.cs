using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z01.Models
{
  [Table("Note")]
  public class Note
  {
    public int NoteID { get; set; }
    public DateTime NoteDate { get; set; } = DateTime.Now;
    public bool Markdown { get; set; }
    [Required]
    [MinLength(1), MaxLength(64)]
    public string Title { get; set; }
    [MinLength(1)]
    public string Description { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }
    public List<NoteCategory> NoteCategories { get; set; }
  }
}
