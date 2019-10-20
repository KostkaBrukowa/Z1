using System;

namespace Z01.Models
{
    public class NoteModel
    {
        public NoteModel()
        {
            Id = "1";
            CreationDate = DateTime.Now;
            Title = "Example title";
            Markdown = true;
            Content = "sample content";
        }

        public string Id { get; }
        public DateTime CreationDate { get; }
        public string Title { get; }
        public bool Markdown { get; }
        public string Content { get; }
    }
}
