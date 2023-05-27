using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;

namespace CleanEjdg.Core.Application.Common
{
    public interface IApplicationDbContext 
    {
        DbSet<Cat> Cats { get; }

        string ConnectionString { get; }

        Task SaveChangesAsync();

    }
}
