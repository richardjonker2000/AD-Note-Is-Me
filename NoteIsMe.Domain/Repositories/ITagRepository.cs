using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Domain.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<Tag>> FindAllMyTagsAsync(int usrID);

        Task DeleteWithForeignKeyAsync(Tag tag);
    }
}
