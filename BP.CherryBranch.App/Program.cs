using Lucet.CherryBranch.App.Components;
using Lucet.CherryBranch.App.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("RepoClient", client =>
{
    string? repoApi = builder.Configuration.GetValue<string>("CherryBranch_API");
    client.BaseAddress = new Uri(repoApi ?? string.Empty);

    //set the timeout to the default (100 seconds) + 10 seconds to allow for the client to timeout with it's request to the on-premise api first
    client.Timeout = TimeSpan.FromSeconds(110);

    client.DefaultRequestHeaders.Add("Content_Type", "application/json");
});

builder.Services.AddSingleton<RepoService>();
builder.Services.AddSingleton<SettingsService>();
builder.Services.AddSingleton<APIService>();

builder.Services.AddScoped<AppState>(provider => new AppState(
    provider.GetRequiredService<APIService>(),
    provider.GetRequiredService<AuthenticationStateProvider>(),
    provider.GetRequiredService<NavigationManager>(),
    builder.Configuration
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
