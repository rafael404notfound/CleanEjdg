using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanEjdg.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
        {

        }

        public DbSet<Cat> Cats => Set<Cat>();

        public void Remove(CatPhoto catPhoto)
        {
            throw new NotImplementedException();
        }

        EntityEntry IApplicationDbContext.Entry<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        void IApplicationDbContext.SaveChanges()
        {
            base.SaveChanges();
        }

        async Task IApplicationDbContext.SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
