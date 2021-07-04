using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Domain.Repositories
{
    public interface ISketchTagsRepository : IRepository<SketchTag>
    {
        Task<List<SketchTag>> FindAllByTagAsyc(int id);
        Task<List<SketchTag>> FindAllBySketchIdAsync(int id);
    }
}
