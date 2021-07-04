using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Repositories
{
    public interface IFolderRepository : IRepository<Folder>
    {
        Task<List<Folder>> FindAllMyFoldersAsync(int usrID);

        Task DeleteWithForeignKeyAsync(Folder folder);
        Task<List<Folder>> FindByUserIdAsync(int userId);

    }
}
