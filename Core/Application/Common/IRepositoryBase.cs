using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Common
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        Task<T> Get(int id);
        //Task Create(T entity);
        void Create(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
