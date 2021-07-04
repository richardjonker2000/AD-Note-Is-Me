using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteIsMe.Domain.Models
{
    public class SketchTag
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public int SketchId { get; set; }
        public Sketch Sketch { get; set; }

    }
}
