using System;
using System.Collections.Generic;
using System.Text;
using NoteIsMe.Domain.Models;

namespace NoteIsMe.Domain.SeedWork
{
    public abstract class GenericNote: Entity
    {
        public  string Title { get; set; }
        public int LastModifierUserId { get; set; }
        public byte[] Content { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public int NotebookId { get; set; }
        public Notebook Notebook { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
