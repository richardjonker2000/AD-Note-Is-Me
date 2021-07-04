using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteIsMe.Domain.Models
{
    public class NoteTag
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public int NoteId { get; set; }
        public Note Note { get; set; }
        

    }
}
