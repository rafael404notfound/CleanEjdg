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

        public async Task<Cat> Get(int id)
        {
            return await DbContext.Cats.FindAsync(id) ?? new Cat();
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
