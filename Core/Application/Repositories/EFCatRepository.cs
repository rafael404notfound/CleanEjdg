using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Domain.Entities;

namespace CleanEjdg.Core.Application.Repositories
{
    public class EFCatRepository : IRepositoryBase<Cat>
    {
        IApplicationDbContext DbContext;
        public EFCatRepository(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Create(Cat entity)
        {
            await DbContext.Cats.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            DbContext.Cats.Remove(new Cat { Id = id });
            await DbContext.SaveChangesAsync();            
        }

        public async Task<Cat> Get(int id)
        {
            return await DbContext.Cats.FindAsync(id) ?? new Cat();
        }

        public IQueryable<Cat> GetAll()
        {
            return DbContext.Cats;
        }

        public async Task Update(Cat entity)
        {
            DbContext.Cats.Update(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
