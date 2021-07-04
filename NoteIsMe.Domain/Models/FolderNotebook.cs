using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteIsMe.Domain.Models
{
    public class FolderNotebook
    {
        public int NoteBookId { get; set; }
        public Notebook NoteBook { get; set; }

        public int FolderId { get; set; }
        public Folder Folder { get; set; }

    }
}
