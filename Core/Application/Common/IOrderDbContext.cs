using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanEjdg.Core.Application.Common
{
    public interface IOrderDbContext
    {
        DbSet<Order> Orders { get; }

        void SaveChanges();
        Task SaveChangesAsync();
        EntityEntry Entry<TEntity>(TEntity entity);
    }
}
