using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;


namespace CleanEjdg.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
        {

        }

        public DbSet<Cat> Cats => Set<Cat>();

        async Task IApplicationDbContext.SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
