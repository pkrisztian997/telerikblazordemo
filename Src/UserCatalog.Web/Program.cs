using Microsoft.AspNetCore.Components;
using UserCatalog.Web.Components;
using UserCatalog.Web.Core;

var builder = WebApplication.CreateBuilder(args);
AddTelerikBlazor(builder);
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
});
builder.Services.AddUserCatalogWebModule(@".\Resources\UserDatabase.txt");

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapControllers();

SetUpDebugErrorHandler(app);

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.MapStaticAssets();

app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

app.Run();

static void AddTelerikBlazor(WebApplicationBuilder builder)
{
    builder.Services.AddTelerikBlazor();
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
}

static void SetUpDebugErrorHandler(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }
}
