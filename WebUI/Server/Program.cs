using Microsoft.AspNetCore.ResponseCompression;
using CleanEjdg.Infrastructure.Persistance;
using CleanEjdg.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Application.Repositories;
using CleanEjdg.Core.Application.Services;
using Microsoft.OpenApi.Models;
using CleanEjdg.Core.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>( opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:CatsConnection"]);
    opts.EnableSensitiveDataLogging(true);
});
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IRepositoryBase<Cat>, EFCatRepository>();
builder.Services.AddScoped<IDateTime, DateTimeServer>();
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

var applicationDbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
CatSeedData.SeedDataBase(applicationDbContext);

app.Run();
