using Factory.Components;
using Factory.Factories;
using Factory.Services;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

var builder = WebApplication.CreateBuilder(args);

// Makes Blazor static assets available even when the app is started directly from bin/Debug.
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SmartDeviceCreator, SmartLightCreator>();
builder.Services.AddSingleton<SmartDeviceCreator, SmartPlugCreator>();
builder.Services.AddSingleton<SmartDeviceCreator, ThermostatCreator>();
builder.Services.AddSingleton<SmartDeviceCreator, SmartFanCreator>();
builder.Services.AddSingleton<SmartHomeService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseStaticFiles();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
