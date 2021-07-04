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
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(NoteismeDbContext db) : base(db)
        {
        }

        public async Task<List<Group>> FindByNotebookIdAsync(int id)
        {
            List<Group> r = new List<Group>();

            List<Group> temp = await _dbContext.Groups.Where(x => x.NotebookId == id).ToListAsync();
            foreach (Group group in temp)
            {
                r.Add(group);
            }

            return r;
        }

        public async Task<List<Group>> FindByUserIdAsync(int userId)
        {
            List<Group> r = new List<Group>();

            List<Group> temp = await _dbContext.Groups.Where(x => x.UserId == userId).ToListAsync();
            foreach (Group group in temp)
            {
                r.Add(group);
            }

            return r;
        }

        public Group getUserNotebookGroup(int notebookID, int userID)
        {
            return _dbContext.Groups.SingleOrDefault(x => x.UserId == userID && x.NotebookId == notebookID);
            
        }

        public async Task<Group> getUserNotebookGroupAsync(int notebookID, int userID)
        {
            Task<Group> group = _dbContext.Groups.SingleOrDefaultAsync(x => x.UserId == userID && x.NotebookId == notebookID);
            return await group;
        }   
    }
}
