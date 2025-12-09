using EAM.Web.Portal.Components;
using EAM.Web.Portal.Services;
using EAM.Infra.IoC;
using Microsoft.AspNetCore.Identity;
using EAM.Core.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Infrastructure (Database, Repositories, Identity)
builder.Services.AddInfrastructure(builder.Configuration);

// Application Services
builder.Services.AddScoped<IProductService, ProductService>();

// HttpContextAccessor e SignInManager para autenticação
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();

// Adiciona autenticação via cookies
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = true;
    })
    .AddCookie(IdentityConstants.ExternalScheme);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication(); // ANTES de UseAuthorization e Antiforgery
app.UseAuthorization(); // Autorização necessária para [Authorize]
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
