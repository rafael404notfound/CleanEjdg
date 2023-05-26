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

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>
    {
        private readonly IContainer _mssqlContainer = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .WithPortBinding(1433, 2000)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SQLCMDUSER", "rafa")
            .WithEnvironment("SQLCMDPASSWORD", "MySecret123")
            .WithEnvironment("MSSQL_SA_PASSWORD", "MySecret123")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(2000))
            .Build();

        public IntegrationTestFactory()
        {
            //_container = Testcontainersbui
        }
        // Gives a fixture an opportunity to configure the application before it gets built
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove AppDbContext
                //var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                //if (descriptor != null) services.Remove(descriptor);

                // Add Db context pointing to test container
                //services.AddDbContext<ApplicationDbContext>(opts =>
                //{
                //    opts.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Cats;MultipleActiveResultSets=True");
                //});
            });
        }
    }
}
