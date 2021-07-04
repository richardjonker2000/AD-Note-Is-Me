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
    public class SketchTagsRepository : Repository<SketchTag>, ISketchTagsRepository
    {
        public SketchTagsRepository(NoteismeDbContext db) : base(db)
        {
        }

        public async Task<List<SketchTag>> FindAllBySketchIdAsync(int id)
        {
            List<SketchTag> sketchTags = await _dbContext.SketchTags.Where(x => x.TagId == id).ToListAsync();
            return sketchTags;
        }

        public async Task<List<SketchTag>> FindAllByTagAsyc(int id)
        {
            List<SketchTag> sketchTag = await _dbContext.SketchTags.Where(x => x.TagId == id).ToListAsync();
            return sketchTag;
        }

    }
}
