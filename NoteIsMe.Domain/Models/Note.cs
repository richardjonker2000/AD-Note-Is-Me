using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteIsMe.Domain.Models
{
    public class Note: GenericNote
    {
        public List<NoteTag> NoteTags { get; set; }


    }
}
