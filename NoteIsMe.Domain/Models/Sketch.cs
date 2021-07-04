using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteIsMe.Domain.Models
{
    public class Sketch: GenericNote
    {
        public List<SketchTag> SketchTags { get; set; }



    }
}
