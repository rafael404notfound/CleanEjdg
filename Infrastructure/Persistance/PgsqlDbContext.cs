using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;


namespace CleanEjdg.Infrastructure.Persistance
{
    public class PgsqlDbContext : DbContext, IApplicationDbContext
    {
        public PgsqlDbContext(DbContextOptions<PgsqlDbContext> opts) : base(opts)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Cat> Cats => Set<Cat>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=CatDb;User Id=postgres;Password=password;");

        async Task IApplicationDbContext.SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
