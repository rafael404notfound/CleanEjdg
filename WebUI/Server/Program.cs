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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
});
builder.Services.AddDbContext<ProductDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
});
builder.Services.AddDbContext<OrderDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:OrderConnection"]);
    opts.EnableSensitiveDataLogging(true);
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
});
builder.Services.AddDbContext<IdentityContext>(opts => opts.UseNpgsql(builder.Configuration["ConnectionStrings:IdentityConnection"]));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PgsqlDbContext>());
builder.Services.AddScoped<IProductDbContext>(provider => provider.GetRequiredService<ProductDbContext>());
builder.Services.AddScoped<IOrderDbContext>(provider => provider.GetRequiredService<OrderDbContext>());
builder.Services.AddScoped<IRepositoryBase<Cat>, EFCatRepository>();
builder.Services.AddScoped<IRepositoryBase<Product>, EFProductRepository>();
builder.Services.AddScoped<IRepositoryBase<Order>, EFOrderRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IDateTimeServer, DateTimeServer>();
builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        //encoding.Ascii worked
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true
    };
});
/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        /*options.Events.OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                context.Token = context.Request.Cookies["X-Access-Token"];
            }
            return Task.CompletedTask;
        };
    });*/
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
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
app.UseSession();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Ejdg Api");
});

//var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//var userManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
//await RoleSeedData.SeedDataBase(roleManager, userManager);
//var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
//CatSeedData.SeedDataBase(dbContext);


app.Run();

// Needed for WebApplicationFactory<Program> in WebUi.Server.IntegrationTests 
// https://www.azureblue.io/asp-net-core-integration-tests-with-test-containers-and-postgres/
public partial class Program { }