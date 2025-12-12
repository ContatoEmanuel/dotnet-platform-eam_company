using System.Net.Http.Json;
using EAM.Core.Application.DTOs.Lead;

namespace EAM.Web.Portal.Services;

public interface ILeadApiService
{
    Task<List<LeadDto>> GetAllLeadsAsync();
    Task<LeadDto?> GetLeadByIdAsync(int id);
    Task<LeadDto?> CreateLeadAsync(LeadCreateDto dto);
    Task<LeadDto?> MarkAsContactedAsync(int id);
    Task<bool> DeleteLeadAsync(int id);
}

public class LeadApiService : ILeadApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LeadApiService> _logger;

    public LeadApiService(HttpClient httpClient, ILogger<LeadApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<LeadDto>> GetAllLeadsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/leads");
            
            if (response.IsSuccessStatusCode)
            {
                var leads = await response.Content.ReadFromJsonAsync<List<LeadDto>>();
                return leads ?? new List<LeadDto>();
            }

            _logger.LogWarning("Falha ao obter leads. Status: {StatusCode}", response.StatusCode);
            return new List<LeadDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter leads da API");
            return new List<LeadDto>();
        }
    }

    public async Task<LeadDto?> GetLeadByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/leads/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LeadDto>();
            }

            _logger.LogWarning("Falha ao obter lead {LeadId}. Status: {StatusCode}", id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter lead {LeadId} da API", id);
            return null;
        }
    }

    public async Task<LeadDto?> CreateLeadAsync(LeadCreateDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/leads", dto);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LeadDto>();
            }

            _logger.LogWarning("Falha ao criar lead. Status: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar lead via API");
            return null;
        }
    }

    public async Task<LeadDto?> MarkAsContactedAsync(int id)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"api/leads/{id}/contact", null);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LeadDto>();
            }

            _logger.LogWarning("Falha ao marcar lead {LeadId} como contatado. Status: {StatusCode}", id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar lead {LeadId} como contatado via API", id);
            return null;
        }
    }

    public async Task<bool> DeleteLeadAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/leads/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar lead {LeadId} via API", id);
            return false;
        }
    }
}
