using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Repositories
{
    public interface ISketchRepository: IRepository<Sketch>
    {
        Task<List<Sketch>> FindSketchesByUserID(int userid);
        Task<List<Sketch>> FindByNotebookAsync(int notebookid);
        Task<List<Sketch>> FindByUserIdAsync(int userId);
    }
}
