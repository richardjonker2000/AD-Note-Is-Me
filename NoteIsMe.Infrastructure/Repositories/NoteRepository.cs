using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.Repositories;

namespace NoteIsMe.Infrastructure.Repositories
{
    public class NoteRepository : Repository<Note>, INoteRepository
    {
        public NoteRepository(NoteismeDbContext db) : base(db)
        {

           
        }

        public async Task<List<Note>> FindNotesByUserID(int userid)
        {
            List<Note> notes = await _dbContext.Notes.Where(x => x.OwnerId == userid).ToListAsync();

            return notes;
        }


        public async Task<List<Note>> FindByNotebookAsync(int notebookid)
        {
            List<Note> r = new List<Note>();

            List<Note> temp = await _dbContext.Notes.Where(x => x.NotebookId == notebookid).ToListAsync();
            foreach (Note note in temp)
            {
                r.Add(note);
            }

            return r;
        }

        public async Task<List<Note>> FindByUserIdAsync(int userId)
        {
            List<Note> r = new List<Note>();

            List<Note> temp = await _dbContext.Notes.Where(x => x.OwnerId == userId).ToListAsync();
            foreach (Note note in temp)
            {
                r.Add(note);
            }

            return r;
        }


    }
}
