using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteIsMe.Domain.Models;
using NoteIsMe.Domain.SeedWork;


namespace NoteIsMe.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T: class
    {
        protected NoteismeDbContext _dbContext;

        public Repository(NoteismeDbContext db)
        {
            _dbContext = db;
        }

        public async Task<T> CreateAsync(T e)
        {
            T r = _dbContext.Set<T>().Add(e).Entity;  // generic
            await _dbContext.SaveChangesAsync();

            return r;
        }

        public async Task<T> DeleteAsync(T e)
        {


            T res = _dbContext.Set<T>().Remove(e).Entity;
            await _dbContext.SaveChangesAsync();
            return res;
        }


        public async Task<T> FindByIdAsync(int id)
        {
            return await _dbContext.FindAsync<T>(id);

        }

        public T FindById(int id)
        {
            return _dbContext.Find<T>(id);

        }

        public async Task<List<T>> FindAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<int> FindTotalCountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }


        public async Task<T> UpdateAsync(T e)
        {
            T res = _dbContext.Set<T>().Update(e).Entity;
            await _dbContext.SaveChangesAsync();
            return res;
        }

        public async Task<T> UpsertAsync(T e)
        {
            T res = null;
            
            try
            {
                res = await this.UpdateAsync(e);
            }
            catch
            {
                res = await this.CreateAsync(e);
            }

            await _dbContext.SaveChangesAsync();
            return res;

        }
    }
}

