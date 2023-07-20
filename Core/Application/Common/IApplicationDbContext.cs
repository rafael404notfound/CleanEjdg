using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanEjdg.Core.Application.Common
{
    public interface IApplicationDbContext 
    {
        DbSet<Cat> Cats { get; }

        void SaveChanges();
        Task SaveChangesAsync();
        EntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
        void Remove(CatPhoto catPhoto);
    }
}
