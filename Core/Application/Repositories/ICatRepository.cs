using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Repositories
{
    public interface ICatRepository
    {
        IQueryable<Cat> GetAll();
        Task<Cat> Get(int id);
        void Create(Cat entity);
        void Update(Cat entity);
        void Delete(Cat entity);
    }
}
