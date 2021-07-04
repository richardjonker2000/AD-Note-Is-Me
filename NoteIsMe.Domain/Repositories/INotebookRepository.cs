using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Domain.Repositories
{
    public interface INotebookRepository : IRepository<Notebook>
    {
        Task<List<Notebook>> FindMyOwnedNotebooksAsync(int userid);
        Task<List<Notebook>> FindAllSharedToMeNotebooksAsync(int userid);
        Task<List<Notebook>> FindAllInFolderAsync(int folderID);
        Task<Notebook> DeleteNotebookAsync(Notebook notebook);
        Task<List<Notebook>> FindByUserIdAsync(int userId);
    }


}
