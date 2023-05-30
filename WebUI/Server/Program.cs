using Microsoft.AspNetCore.ResponseCompression;
using CleanEjdg.Infrastructure.Persistance;
using CleanEjdg.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Application.Repositories;
using CleanEjdg.Core.Application.Services;
using Microsoft.OpenApi.Models;
using CleanEjdg.Core.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PgsqlDbContext>( opts =>
{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:PgsqlConnection"]);
    //opts.UseMySql(builder.Configuration["ConnectionStrings:CatsDockerConnection"], 
    //    ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:CatsDockerConnection"]),
    //    opts => opts.EnableRetryOnFailure(
    //        maxRetryCount: 5,
    //        maxRetryDelay: System.TimeSpan.FromSeconds(30),
    //        errorNumbersToAdd: null));
    //opts.EnableSensitiveDataLogging(true);
    //opts.UseSqlServer(builder.Configuration["ConnectionStrings:CatsConnection"]);
    opts.EnableSensitiveDataLogging(true);
});
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PgsqlDbContext>());
builder.Services.AddScoped<IRepositoryBase<Cat>, EFCatRepository>();
builder.Services.AddScoped<IDateTimeServer, DateTimeServer>();
builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ejdg Api", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Ejdg Api");
});

//var applicationDbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<PgsqlDbContext>();
//CatSeedData.SeedDataBase(applicationDbContext);

app.Run();

// Needed for WebApplicationFactory<Program> in WebUi.Server.IntegrationTests 
// https://www.azureblue.io/asp-net-core-integration-tests-with-test-containers-and-postgres/
public partial class Program { }