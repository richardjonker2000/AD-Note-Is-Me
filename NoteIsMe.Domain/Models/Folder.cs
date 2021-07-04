using System;
using System.Collections.Generic;
using System.Text;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Models
{
    public class Folder: Entity
    {
        public string Name { get; set; }
        
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string IconURL { get; set; }
        
        public List<FolderNotebook> FolderNotebooks { get; set; }
    }
}
