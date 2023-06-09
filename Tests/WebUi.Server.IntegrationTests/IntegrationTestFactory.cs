﻿using Microsoft.AspNetCore.Hosting;
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
                    opts.UseNpgsql(_container.GetConnectionString());
                });

                // Ensure schema gets created
                services.EnsureDbCreated<PgsqlDbContext>();
            });
        }

        public async Task InitializeAsync() => await _container.StartAsync();

        public new async Task DisposeAsync() => await _container.DisposeAsync();

        public DbContextOptions<PgsqlDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<PgsqlDbContext>().UseNpgsql(_container.GetConnectionString()).Options;
        }

        public void SetDbInitialState(IEnumerable<Cat> cats)
        {
            var dbOptions = GetDbContextOptions();

            using (var context = new PgsqlDbContext(dbOptions))
            {
                // Remove all cats from db
                foreach(Cat c in context.Cats)
                {
                    context.Remove(c);
                }
                context.SaveChanges();

                // Store cats in db
                context.Cats.AddRange(cats);
                context.SaveChanges();
            }
        }
    }
}
