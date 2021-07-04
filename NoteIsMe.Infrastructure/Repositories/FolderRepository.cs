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
    public class FolderRepository: Repository<Folder>, IFolderRepository
    {
        public FolderRepository(NoteismeDbContext db) : base(db)
        {

        }

        public async Task<List<Folder>> FindAllMyFoldersAsync(int usrID)
        {
            List<Folder> folders = await _dbContext.Folders.Where(x => x.OwnerId == usrID).ToListAsync();

            return folders;
        }

        public async Task DeleteWithForeignKeyAsync(Folder folder)
        {
            List<FolderNotebook> folderNotebooks = await _dbContext.FolderNotebooks.Where(x => x.FolderId == folder.Id).ToListAsync();
            foreach (FolderNotebook f in folderNotebooks)
            {
                _dbContext.FolderNotebooks.Remove(f);
            }
            _dbContext.Folders.Remove(folder);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Folder>> FindByUserIdAsync(int userId)
        {
            List<Folder> r = new List<Folder>();

            List<Folder> temp = await _dbContext.Folders.Where(x => x.OwnerId == userId).ToListAsync();
            foreach (Folder folder in temp)
            {
                r.Add(folder);
            }

            return r;
        }
    }
}
