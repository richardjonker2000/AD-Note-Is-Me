using System;
using System.Collections.Generic;
using System.Text;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Models
{
    public class Group
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public Notebook Notebook{ get; set; }
        public int NotebookId { get; set; }

        public bool ViewPermission { get; set; }
        public bool EditPermission { get; set; }
        public bool SharePermission { get; set; }
    }
}
