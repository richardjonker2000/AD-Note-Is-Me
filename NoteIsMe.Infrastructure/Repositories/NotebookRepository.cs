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
    public class NotebookRepository : Repository<Notebook>, INotebookRepository
    {
        public NotebookRepository(NoteismeDbContext db) : base(db)
        {
        }

        public async Task<List<Notebook>> FindMyOwnedNotebooksAsync(int userid)
        {
            List<Notebook> noteboooks = await _dbContext.Notebooks.Where(x => x.OwnerId == userid).ToListAsync();
            return noteboooks;
        }

        public async Task<List<Notebook>> FindAllSharedToMeNotebooksAsync(int userid)
        {
            
            List<Group> groups = await _dbContext.Groups.Where(x => x.UserId == userid && x.ViewPermission == true).ToListAsync();
            List<Notebook> noteboooks = new List<Notebook>();

            foreach (Group group in groups)
            {

                Notebook test = await _dbContext.Notebooks.SingleOrDefaultAsync(x => x.Id == group.NotebookId);
                noteboooks.Add(test);

            }


            return noteboooks;
        }

        public async Task<List<Notebook>> FindAllInFolderAsync(int folderID)
        {
            List<FolderNotebook> folderNotebooks = await _dbContext.FolderNotebooks.Where(x => x.FolderId == folderID).ToListAsync();
            List<Notebook> notebooks = new List<Notebook>();
            foreach (FolderNotebook f in folderNotebooks)
            {
                Notebook test = await _dbContext.Notebooks.SingleOrDefaultAsync(x => x.Id == f.NoteBookId);
                notebooks.Add(test);
            }

            return notebooks;

        }

        public async Task<Notebook> DeleteNotebookAsync(Notebook notebook)
        {
            _dbContext.Set<Group>().RemoveRange(notebook.Groups);
            _dbContext.Set<Note>().RemoveRange(notebook.Notes);
            _dbContext.Set<Sketch>().RemoveRange(notebook.Sketches);
            _dbContext.Set<FolderNotebook>().RemoveRange(notebook.FolderNotebooks);


            Notebook res = _dbContext.Set<Notebook>().Remove(notebook).Entity;
            await _dbContext.SaveChangesAsync();

            return res;
        }

        public async Task<List<Notebook>> FindByUserIdAsync(int userId)
        {
            List<Notebook> r = new List<Notebook>();

            List<Notebook> temp = await _dbContext.Notebooks.Where(x => x.OwnerId == userId).ToListAsync();
            foreach (Notebook notebook in temp)
            {
                r.Add(notebook);
            }

            return r;
        }
    }
}
