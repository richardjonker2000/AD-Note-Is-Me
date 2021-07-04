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
    public class FolderNotebooksRepository : Repository<FolderNotebook>, IFolderNotebooksRepository
    {
        public FolderNotebooksRepository(NoteismeDbContext db) : base(db)
        {
        }

        public async Task<List<FolderNotebook>> FindByFolderIdAsync(int id)
        {
            List<FolderNotebook> r = new List<FolderNotebook>();

            List<FolderNotebook> temp = await _dbContext.FolderNotebooks.Where(x => x.FolderId == id).ToListAsync();
            foreach (FolderNotebook fn in temp)
            {
                r.Add(fn);
            }

            return r;
        }

        public async Task<List<FolderNotebook>> FindByNotebookIdAsync(int id)
        {
            List<FolderNotebook> r = new List<FolderNotebook>();

            List<FolderNotebook> temp = await _dbContext.FolderNotebooks.Where(x => x.NoteBookId == id).ToListAsync();
            foreach (FolderNotebook fn in temp)
            {
                r.Add(fn);
            }

            return r;
        }
    }
}
