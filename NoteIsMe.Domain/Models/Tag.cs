using System;
using System.Collections.Generic;
using System.Text;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Models
{
    public class Tag: Entity
    {
        public string Name { get; set; }

        public string Color { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }


        public List<NoteTag> NoteTags { get; set; }
        public List<SketchTag> SketchTags { get; set; }
    }
}
