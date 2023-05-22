using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Domain.Entities;

namespace CleanEjdg.Core.Application.Repositories
{
    public class EFCatRepository : ICatRepository
    {
        IApplicationDbContext DbContext;
        public EFCatRepository(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Create(Cat entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cat entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Cat> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Cat> GetAll()
        {
            return DbContext.Cats;
        }

        public void Update(Cat entity)
        {
            throw new NotImplementedException();
        }
    }
}
