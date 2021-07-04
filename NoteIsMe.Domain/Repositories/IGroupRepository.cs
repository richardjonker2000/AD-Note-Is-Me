using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Domain.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Group getUserNotebookGroup(int notebookID, int userID);
        Task<Group> getUserNotebookGroupAsync(int notebookID, int userID);
        Task<List<Group>> FindByUserIdAsync(int userId);
        Task<List<Group>> FindByNotebookIdAsync(int id);
    }
}
