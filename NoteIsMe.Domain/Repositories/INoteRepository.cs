using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Repositories
{
    public interface INoteRepository: IRepository<Note>
    {
        Task<List<Note>> FindNotesByUserID(int userid);
        Task<List<Note>> FindByNotebookAsync(int notebookid);
        Task<List<Note>> FindByUserIdAsync(int userId);
    }
}
