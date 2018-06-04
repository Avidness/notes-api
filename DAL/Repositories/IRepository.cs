using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace notes_api.DAL.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll(); 
        Task<T> Get(int id); 
        void Create(T entity); 
        void Delete(int id); 
        void Update(T entity); 
    }
}