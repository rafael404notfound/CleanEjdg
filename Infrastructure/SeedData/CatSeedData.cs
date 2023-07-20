using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;
using CleanEjdg.Infrastructure.Persistance;

namespace CleanEjdg.Infrastructure.SeedData
{
    public static class CatSeedData
    {
        public static void SeedDataBase(IApplicationDbContext context)
        {
            //context.Database.Migrate();
            var date1 = new DateTime(2021, 2, 23);
            DateTime.SpecifyKind(date1, DateTimeKind.Utc);
            if (context.Cats.Count() == 0)
            {
                context.Cats.AddRange(
                    new Cat
                    {
                        Name = "Susan",
                        DateOfBirth = DateTime.SpecifyKind(new DateTime(2021, 2, 23), DateTimeKind.Utc),
                        HasChip = true,
                        IsSterilized = true,
                        IsVaccinated = true,
                    },
                    new Cat
                    {
                        Name = "Yuki",
                        DateOfBirth = DateTime.SpecifyKind(new DateTime(2022, 8, 15), DateTimeKind.Utc),
                        HasChip = true,
                        IsSterilized = true,
                        IsVaccinated = false,
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
