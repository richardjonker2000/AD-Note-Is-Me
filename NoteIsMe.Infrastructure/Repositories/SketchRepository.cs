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
    public class SketchRepository : Repository<Sketch>, ISketchRepository
    {
        public SketchRepository(NoteismeDbContext db) : base(db)
        {
        }


        public async Task<List<Sketch>> FindSketchesByUserID(int userid)
        {
            List<Sketch> sketches = await _dbContext.Sketches.Where(x => x.OwnerId == userid).ToListAsync();

            return sketches;
        }



        public async Task<List<Sketch>> FindByNotebookAsync(int notebookid)
        {
            List<Sketch> r = new List<Sketch>();

            List<Sketch> temp = await _dbContext.Sketches.Where(x => x.NotebookId == notebookid).ToListAsync();
            foreach (Sketch sketch in temp)
            {
                r.Add(sketch);
            }

            return r;
        }

        public async Task<List<Sketch>> FindByUserIdAsync(int userId)
        {
            List<Sketch> r = new List<Sketch>();

            List<Sketch> temp = await _dbContext.Sketches.Where(x => x.OwnerId == userId).ToListAsync();
            foreach (Sketch sketch in temp)
            {
                r.Add(sketch);
            }

            return r;
        }
    }
}
