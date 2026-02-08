using Microsoft.AspNetCore.Components;
using UserCatalog.Web.Components;

var builder = WebApplication.CreateBuilder(args);
AddTelerikBlazor(builder);
AddControllerSupport(builder);

var app = builder.Build();
MapControllers(app);
SetUpDebugErrorHandler(app);
SetUpComponents(app);
app.Run();

static void AddTelerikBlazor(WebApplicationBuilder builder)
{
    builder.Services.AddTelerikBlazor();
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
}

static void AddControllerSupport(WebApplicationBuilder builder)
{
    builder.Services.AddHttpClient();
    builder.Services.AddControllers();
    builder.Services.AddScoped(sp =>
    {
        var navigation = sp.GetRequiredService<NavigationManager>();
        return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
    });
}

static void MapControllers(WebApplication app)
{
    app.UseRouting();
    app.UseAntiforgery();
    app.MapControllers();
}

static void SetUpDebugErrorHandler(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }
}

static void SetUpComponents(WebApplication app)
{
    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();
    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
}
