using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers;
using Testcontainers.PostgreSql;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class IntegrationTestFactory : WebApplicationFactory<Program> , IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container;

        public IntegrationTestFactory()
        {
            _container = new PostgreSqlBuilder()
                .WithDatabase("test_db")
                .WithUsername("postgres")
                .WithPassword("password")
                .WithImage("postgres:14.7")
                .WithCleanUp(true)
                .Build();
        }
        // Gives a fixture an opportunity to configure the application before it gets built
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove AppDbContext
                services.RemoveDbContext<PgsqlDbContext>();

                // Add Db context pointing to test container
                services.AddDbContext<PgsqlDbContext>(opts =>
                {
                    string connectionString = _container.GetConnectionString();
                    opts.UseNpgsql(_container.GetConnectionString());
                });

                // Ensure schema gets created
                services.EnsureDbCreated<PgsqlDbContext>();

                // Set initial state
                RestoreInitialState();
            });
        }

        public async Task InitializeAsync() => await _container.StartAsync();

        public new async Task DisposeAsync() => await _container.DisposeAsync();

        public DbContextOptions<PgsqlDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<PgsqlDbContext>().UseNpgsql(_container.GetConnectionString()).Options;
        }

        public void RestoreInitialState()
        {
            var dbOptions = GetDbContextOptions();

            using (var context = new PgsqlDbContext(dbOptions))
            {
                // Remove cats from db;
                foreach(Cat c in context.Cats)
                {
                    context.Remove(c);
                }
                context.SaveChanges();
                
                // Add seed data
                context.Cats.AddRange(
                    new Cat
                    {
                        Name = "Susan",
                        DateOfBirth = new DateTime(2021, 2, 23),
                        HasChip = true,
                        IsSterilized = true,
                        IsVaccinated = true
                    },
                    new Cat
                    {
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
