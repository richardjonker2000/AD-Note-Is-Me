using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Domain.Repositories
{
    public interface INoteTagsRepository : IRepository<NoteTag>
    {
        Task<List<NoteTag>> FindAllByTagAsyc(int id);
        Task<List<NoteTag>> FindAllByNoteIdAsync(int id);
    }
}
