using Scalar.AspNetCore;
using EAM.Infra.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EAM.Core.Application.Services.Interfaces;
using EAM.Core.Application.Services;
using EAM.Web.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Infrastructure (Database, Repositories, Identity)
builder.Services.AddInfrastructure(builder.Configuration);

// HttpContextAccessor necessário para SignInManager
builder.Services.AddHttpContextAccessor();

// Identity Managers
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.UserManager<EAM.Core.Domain.Entities.ApplicationUser>>();
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.SignInManager<EAM.Core.Domain.Entities.ApplicationUser>>();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IResumeService, ResumeService>();

// Translation Service - Suporta múltiplos provedores via configuração
var translationProvider = builder.Configuration["Translation:Provider"] ?? "azure";
var translationApiKey = builder.Configuration["Translation:ApiKey"];
var translationRegion = builder.Configuration["Translation:Region"] ?? "eastus";

if (translationProvider.Equals("azure", StringComparison.OrdinalIgnoreCase))
{
    if (string.IsNullOrWhiteSpace(translationApiKey))
    {
        throw new InvalidOperationException(
            "Azure Translator API Key is required when using 'azure' provider. " +
            "Configure in appsettings.json or environment variable Translation__ApiKey");
    }

    builder.Services.AddHttpClient<ITranslationService>((sp, client) =>
    {
        var logger = sp.GetRequiredService<ILogger<AzureTranslatorService>>();
        return new AzureTranslatorService(client, logger, translationApiKey, translationRegion);
    });
}
else if (translationProvider.Equals("google", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddHttpClient<ITranslationService, GoogleTranslationService>()
        .ConfigureHttpClient(client => 
        {
            client.DefaultRequestHeaders.Add("User-Agent", "EAM-Platform/1.0");
        });
    
    // Registra a factory se tiver API key
    if (!string.IsNullOrWhiteSpace(translationApiKey))
    {
        builder.Services.AddScoped<ITranslationService>(sp =>
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
            var logger = sp.GetRequiredService<ILogger<GoogleTranslationService>>();
            return new GoogleTranslationService(httpClient, logger, translationApiKey);
        });
    }
}
else
{
    throw new InvalidOperationException($"Translation provider '{translationProvider}' is not supported. Use 'azure' or 'google'.");
}

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ANTES de UseAuthorization
app.UseAuthorization();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
