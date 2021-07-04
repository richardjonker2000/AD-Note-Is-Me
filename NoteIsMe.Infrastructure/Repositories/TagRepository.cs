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
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(NoteismeDbContext db) : base(db)
        {
        }


        public async Task<List<Tag>> FindAllMyTagsAsync(int usrID)
        {
            List<Tag> tags = await _dbContext.Tags.Where(x => x.UserId == usrID).ToListAsync();

            return tags;
        }

        public async Task DeleteWithForeignKeyAsync(Tag tag)
        {
            List<NoteTag> Note = await _dbContext.NoteTags.Where(x => x.TagId == tag.Id).ToListAsync();
            foreach (NoteTag f in Note)
            {
                _dbContext.NoteTags.Remove(f);
            }


            List<SketchTag> skecth = await _dbContext.SketchTags.Where(x => x.TagId == tag.Id).ToListAsync();


            foreach (SketchTag f in skecth)
            {
                _dbContext.SketchTags.Remove(f);
            }
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }
    }
}
