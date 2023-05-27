using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;
using CleanEjdg.Infrastructure.Persistance;

namespace CleanEjdg.Infrastructure.SeedData
{
    public static class CatSeedData
    {
        public static void SeedDataBase(PgsqlDbContext context)
        {
            //context.Database.Migrate();
            if(context.Cats.Count() == 0)
            {
                context.Cats.AddRange(
                    new Cat { 
                        Name = "Susan", 
                        DateOfBirth = new DateTime(2021, 2, 23),
                        HasChip = true,
                        IsSterilized = true,
                        IsVaccinated = true
                    },
                    new Cat { 
                        Name = "Yuki", 
                        DateOfBirth = new DateTime(2022, 8, 15),
                        HasChip = true,
                        IsSterilized = true,
                        IsVaccinated = false
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
