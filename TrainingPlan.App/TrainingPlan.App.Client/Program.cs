using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TrainingPlan.App.Client;
using TrainingPlan.App.Client.Code.Authentication;
using TrainingPlan.App.Client.Code.Services;
using static TrainingPlan.App.Client.Pages.Login;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//builder.Services.AddAuthorizationCore();
//builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<TraningPlanUserService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();




await builder.Build().RunAsync();
