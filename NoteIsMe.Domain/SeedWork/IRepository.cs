using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.Domain.SeedWork
{
    public interface IRepository<T> 
    {
        Task<T> CreateAsync(T e);
        Task<T> UpdateAsync(T e);
        Task<T> DeleteAsync(T e);
        Task<List<T>> FindAllAsync();

        Task<T> FindByIdAsync(int id);
        T FindById(int id);
        Task<T> UpsertAsync(T e);

        Task<int> FindTotalCountAsync();

    }
}
