using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Models
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }


        public List<Group> Groups { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Notebook> Notebooks {get; set;}
        public List<Note> Notes { get; set; }
        public List<Sketch> Sketches { get; set; }
        public List<Folder> Folders { get; set; }

        
    }
}
