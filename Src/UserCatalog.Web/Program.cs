using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;
using UserCatalog.Web.Components;
using UserCatalog.Web.Core;

var builder = WebApplication.CreateBuilder(args);
AddTelerikBlazor(builder);
AddControllerSupport(builder);
builder.Services.AddUserCatalogWebModule(@".\Resources\UserDatabase.txt");

// -----
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");
builder.Services.AddHttpContextAccessor();
// -----

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// -----
app.Use((ctx, next) =>
{
    var authCookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x?.StartsWith("auth=") ?? false);

    if (authCookie == null)
    {
        return next();
    }

    var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
    var protector = idp.CreateProtector("auth-cookie");
    var protectedPayload = authCookie?.Split("=").Last();
    var payload = protector.Unprotect(protectedPayload);
    var payloadParts = payload.Split(":");
    var key = payloadParts[0];
    var value = payloadParts[1];

    var claims = new List<Claim>()
    {
        new Claim(key, value)
    };
    var identity = new ClaimsIdentity(claims);
    ctx.User = new ClaimsPrincipal(identity);

    return next();
});

app.MapGet("/username", (HttpContext ctx, IDataProtectionProvider idp) =>
{
    return ctx.User.FindFirst("usr")?.Value ?? "NO AUTH COOKIE CAN BE FOUND";
});

//app.MapGet("/loginA", async (HttpContext ctx) =>
//{
//    var claims = new List<Claim>()
//        {
//            new Claim("usr", "krisz")
//        };
//    var identity = new ClaimsIdentity(claims, "cookie");
//    var user = new ClaimsPrincipal(identity);

//    await ctx.SignInAsync("cookie", user);
//    return "ok";
//});
// -----

app.UseAntiforgery();
app.MapControllers();

SetUpDebugErrorHandler(app);

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.MapStaticAssets();

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
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
}
