using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanEjdg.Infrastructure.Persistance
{
    public class ProductDbContext : DbContext, IProductDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> opts) : base(opts)
        {

        }
        static ProductDbContext()
        {
            
        }
        public DbSet<Product> Products => Set<Product>();

        void IProductDbContext.SaveChanges()
        {
            base.SaveChanges();
        }

        async Task IProductDbContext.SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        EntityEntry IProductDbContext.Entry<TEntity>(TEntity entity)
        {
            return base.Entry(entity);
        }

        public void Remove(ProductPhoto photo)
        {
            base.Remove(photo);
        }
    }
}
