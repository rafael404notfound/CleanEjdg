using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanEjdg.Infrastructure.Persistance
{
    public class PgsqlDbContext : DbContext, IApplicationDbContext
    {
        //public PgsqlDbContext(DbContextOptions<PgsqlDbContext> opts) : base(opts)
        //{
        //    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        //    ConnectionString = this.Database.GetConnectionString() ?? "";
        //}

        public PgsqlDbContext(DbContextOptions<PgsqlDbContext> opts) : base(opts)
        {
            
        }
        static PgsqlDbContext()
        {
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DbSet<Cat> Cats => Set<Cat>();

        void IApplicationDbContext.SaveChanges()
        {
            base.SaveChanges();
        }

        async Task IApplicationDbContext.SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        EntityEntry IApplicationDbContext.Entry<TEntity>(TEntity entity)
        {
            return base.Entry(entity);
        }

        public void Remove(CatPhoto catPhoto)
        {
            base.Remove(catPhoto);
        }
    }
}
