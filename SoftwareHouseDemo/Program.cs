using Microsoft.AspNetCore.Components;
using SoftwareHouseDemo.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTelerikBlazor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// -----
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
});
// -----

var app = builder.Build();

// -----
app.UseRouting();
app.UseAntiforgery();
app.MapControllers();
// -----

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


/*
 
 using SoftwareHouseDemo.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTelerikBlazor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

 */
