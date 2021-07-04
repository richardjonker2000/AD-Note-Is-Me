using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Domain.Repositories
{
    public interface IFolderNotebooksRepository : IRepository<FolderNotebook>
    {
        Task<List<FolderNotebook>> FindByFolderIdAsync(int id);
        Task<List<FolderNotebook>> FindByNotebookIdAsync(int id);
    }
}
