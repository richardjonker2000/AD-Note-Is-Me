using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.Repositories;

namespace NoteIsMe.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(NoteismeDbContext db) : base(db)
        {
        }

        public async Task DeleteUserAsync(User user)
        {
            _dbContext.Notes.RemoveRange(user.Notes);
            _dbContext.Sketches.RemoveRange(user.Sketches);
            _dbContext.Notebooks.RemoveRange(user.Notebooks);
            _dbContext.Folders.RemoveRange(user.Folders);
            _dbContext.Groups.RemoveRange(user.Groups);
            _dbContext.Tags.RemoveRange(user.Tags);

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

        }

        public async Task<User> FindByEmailAsync(string usremail)
        {
            User r = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == usremail);
            
            return r;
        }

        //public async Task<User> FindByUserID(int id)
        //{
        //    User r = await _dbContext.Users.FindAsync(id);

        //    return r;
        //}
    }
}