using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebUI.Client;
using MudBlazor.Services;
using CleanEjdg.Core.Application.Services;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddScoped<IDateTimeServer, DateTimeServer>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddMudServices();



await builder.Build().RunAsync();
