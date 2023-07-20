using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanEjdg.Core.Application.Common
{
    public interface IProductDbContext
    {
        DbSet<Product> Products { get; }

        void SaveChanges();
        Task SaveChangesAsync();
        EntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
        void Remove(ProductPhoto photo);
    }
}
