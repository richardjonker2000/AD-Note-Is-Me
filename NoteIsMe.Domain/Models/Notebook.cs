using System;
using System.Collections.Generic;
using System.Text;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Models
{
    public class Notebook : Entity
    {
        public string Title { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public List<Group> Groups { get; set; }
        public List<FolderNotebook> FolderNotebooks {get; set;}   
        public List<Sketch> Sketches { get; set; }
        public List<Note> Notes { get; set; }
    }
}
