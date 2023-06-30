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
using Microsoft.AspNetCore.Identity;
using CleanEjdg.Core.Domain.Dtos;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class IntegrationTestFactory : WebApplicationFactory<Program> , IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container;
        private readonly PostgreSqlContainer _identityContainer;

        public IntegrationTestFactory()
        {
            _container = new PostgreSqlBuilder()
                .WithDatabase("test_db")
                .WithUsername("postgres")
                .WithPassword("password")
                .WithImage("postgres:14.7")
                .WithCleanUp(true)
                .Build();

            _identityContainer = new PostgreSqlBuilder()
                .WithDatabase("identity_test_db")
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
                //services.RemoveDbContext<IdentityContext>();

                /*
                //Remove UserManager
                var userStore = services.SingleOrDefault(d => d.ServiceType == typeof(IUserStore<IdentityUser>));
                if (userStore != null)services.Remove(userStore);
                var roleStore = services.SingleOrDefault(d => d.ServiceType == typeof(IRoleStore<IdentityRole>));
                if (roleStore != null) services.Remove(roleStore);

                var userManager = services.SingleOrDefault(d => d.ServiceType == typeof(UserManager<IdentityUser>));
                if (userManager != null) services.Remove(userManager);
                var roleManager = services.SingleOrDefault(d => d.ServiceType == typeof(RoleManager<IdentityRole>));
                if (roleManager != null) services.Remove(roleManager); */

                // Add Db context pointing to test container
                services.AddDbContext<PgsqlDbContext>(opts =>
                {
                    opts.UseNpgsql(_container.GetConnectionString());
                });

                /*
                // Add Identity
                services.AddDbContext<IdentityContext>(opts =>
                {
                    opts.UseNpgsql(_identityContainer.GetConnectionString());
                });
                services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
                */

                // Ensure schema gets created
                services.EnsureDbCreated<PgsqlDbContext>();
                //services.EnsureDbCreated<IdentityContext>();
            });
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            await _identityContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _container.DisposeAsync();
            await _identityContainer.DisposeAsync();
        }

        public DbContextOptions<PgsqlDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<PgsqlDbContext>().UseNpgsql(_container.GetConnectionString()).Options;
        }

        public async Task SetDbInitialState(IEnumerable<Cat>? cats = null, IEnumerable<UserDto>? userDtos = null, IEnumerable<IdentityRole>? roles = null)
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
                if ( cats != null)
                {
                    context.Cats.AddRange(cats);
                    context.SaveChanges();
                }
            }
            /*
            var userManager = Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Remove all users from identityDb
            foreach (var user in userManager.Users)
            {
                userManager.DeleteAsync(user).Wait();
            }

            // Remove all roles from identityDb
            foreach (var role in roleManager.Roles)
            {
                roleManager.DeleteAsync(role).Wait();
            }

            // Add roles
            if (roles != null)
            {
                foreach(var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }
            }

            // Add users
            if(userDtos != null)
            {
                foreach(var userDto in userDtos)
                {
                    var user = new IdentityUser { UserName = userDto.UserName, Email = userDto.Email };
                    userManager.CreateAsync(user, "Secret123$").Wait();
                    userManager.AddToRolesAsync(user, userDto.Roles).Wait();
                }
            }*/
        }
    }
}
