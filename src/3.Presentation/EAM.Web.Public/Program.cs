using EAM.Infra.IoC;
using Microsoft.AspNetCore.Identity;
using EAM.Core.Domain.Entities;
using EAM.Core.Application.Services.Interfaces;
using EAM.Web.Public.Services;
using EAM.Web.Public.Data;
using EAM.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Infrastructure (Database, Repositories, Identity)
builder.Services.AddInfrastructure(builder.Configuration);

// HttpClient para consumir a API
builder.Services.AddHttpClient();

// Application Services
builder.Services.AddScoped<IProjectService, ProjectService>();

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

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Apply pending migrations
        context.Database.Migrate();
        
        // Seed database
        await DbInitializer.SeedProjectsAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // ANTES de UseAuthorization
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
