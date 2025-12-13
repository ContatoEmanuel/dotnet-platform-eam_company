using EAM.Core.Application.DTOs.Resume;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EAM.Web.Public.Controllers;

public class ResumeController : BaseController
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public ResumeController(
        IOptions<AppSettings> appSettings,
        IHttpClientFactory httpClientFactory,
        IOptions<ApiSettings> apiSettings) : base(appSettings)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiBaseUrl = apiSettings.Value.BaseUrl;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/resume/full");
            
            if (!response.IsSuccessStatusCode)
            {
                // Se a API falhar, retorna view com dados vazios
                return View(new ResumeDto());
            }

            var json = await response.Content.ReadAsStringAsync();
            var resume = JsonSerializer.Deserialize<ResumeDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(resume ?? new ResumeDto());
        }
        catch (Exception ex)
        {
            // Log error (você pode injetar ILogger aqui)
            Console.WriteLine($"Error fetching resume: {ex.Message}");
            return View(new ResumeDto());
        }
    }
}
