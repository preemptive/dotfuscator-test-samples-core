using Microsoft.Extensions.Configuration;
using PreEmptive.Dotfuscator.Samples.Blazor;
using PreEmptive.Dotfuscator.Samples.Blazor.Components;
using PreEmptive.Dotfuscator.Samples.Blazor.Services;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

ServiceManager.Services.AddStepsProcessors();
ServiceManager.Services.AddTransient<PartiallyUsedTestService>();

ConfigurationManager.Builder
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile($"Core/{Constants.CoreAppsettings}")
    .AddJsonFile("appsettings.json", optional: true);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => new HttpClient());
builder.Services.AddSingleton<WorkflowService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(PreEmptive.Dotfuscator.Samples.Blazor.Client._Imports).Assembly);

app.Run();
