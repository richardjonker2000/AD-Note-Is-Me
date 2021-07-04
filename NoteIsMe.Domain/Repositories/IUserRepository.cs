using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;

namespace NoteIsMe.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
        Task DeleteUserAsync(User user);
        //Task<User> FindByUserID(int id);
    }
}
