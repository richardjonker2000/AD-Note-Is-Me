using Microsoft.EntityFrameworkCore;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Infrastructure.Repositories
{
    public class NoteTagsRepository : Repository<NoteTag>, INoteTagsRepository
    {
        public NoteTagsRepository(NoteismeDbContext db) : base(db)
        {
            
        }
        public async Task<List<NoteTag>> FindAllByTagAsyc(int id)
        {
            List<NoteTag> noteTags = await _dbContext.NoteTags.Where(x => x.TagId== id).ToListAsync();
            return noteTags;
        }

        public async Task<List<NoteTag>> FindAllByNoteIdAsync(int id)
        {
            List<NoteTag> noteTags = await _dbContext.NoteTags.Where(x => x.NoteId == id).ToListAsync();
            return noteTags;
        }
    }
}
