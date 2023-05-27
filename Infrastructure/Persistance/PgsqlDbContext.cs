using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;


namespace CleanEjdg.Infrastructure.Persistance
{
    public class PgsqlDbContext : DbContext, IApplicationDbContext
    {
        public string ConnectionString { get; set; }
        public PgsqlDbContext(DbContextOptions<PgsqlDbContext> opts) : base(opts)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            ConnectionString = this.Database.GetConnectionString() ?? "";
        }

        public DbSet<Cat> Cats => Set<Cat>();

        async Task IApplicationDbContext.SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
