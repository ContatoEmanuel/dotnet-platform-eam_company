using EAM.Core.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace EAM.Core.Application.Services
{
    /// <summary>
    /// Implementação de tradução automática usando Azure Translator.
    /// Suporta free tier: 2 milhões de caracteres por mês de graça!
    /// 
    /// Configuração:
    /// 1. Criar recurso "Translator" no Azure Portal (gratuito!)
    /// 2. Copiar a chave de acesso
    /// 3. Colocar em appsettings.json:
    ///    {
    ///      "Translation": {
    ///        "Provider": "azure",
    ///        "ApiKey": "sua-chave-aqui",
    ///        "Region": "eastus"  // ou outra região
    ///      }
    ///    }
    /// </summary>
    public class AzureTranslatorService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AzureTranslatorService> _logger;
        private readonly string _apiKey;
        private readonly string _region;
        private const string AzureTranslatorApiUrl = "https://api.cognitive.microsofttranslator.com/translate";

        public AzureTranslatorService(HttpClient httpClient, ILogger<AzureTranslatorService> logger, string apiKey, string region = "eastus")
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Azure Translator API Key is required");

            _httpClient = httpClient;
            _logger = logger;
            _apiKey = apiKey;
            _region = region;

            // Configura headers padrão
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", _region);
        }

        public async Task<string> TranslatePortugueseToEnglishAsync(string textToTranslate)
        {
            if (string.IsNullOrWhiteSpace(textToTranslate))
                return textToTranslate;

            try
            {
                var results = await TranslatePortugueseToEnglishBatchAsync(new List<string> { textToTranslate });
                return results.FirstOrDefault() ?? textToTranslate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao traduzir texto via Azure Translator: {Text}", textToTranslate);
                return textToTranslate; // Retorna original em caso de erro
            }
        }

        public async Task<List<string>> TranslatePortugueseToEnglishBatchAsync(List<string> textsToTranslate)
        {
            if (textsToTranslate == null || textsToTranslate.Count == 0)
                return new List<string>();

            try
            {
                var results = new List<string>();

                // Azure Translator aceita múltiplos textos em uma única requisição
                var body = textsToTranslate.Select(text => new { Text = text }).ToList();

                var uri = $"{AzureTranslatorApiUrl}?api-version=3.0&from=pt&to=en&textType=html";

                var response = await _httpClient.PostAsJsonAsync(uri, body);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);

                foreach (var element in jsonDocument.RootElement.EnumerateArray())
                {
                    if (element.TryGetProperty("translations", out var translations))
                    {
                        foreach (var translation in translations.EnumerateArray())
                        {
                            if (translation.TryGetProperty("text", out var text))
                            {
                                results.Add(text.GetString() ?? "");
                            }
                        }
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao traduzir lote de {Count} textos via Azure Translator", textsToTranslate.Count);
                return textsToTranslate; // Retorna originals em caso de erro
            }
        }

        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                // Tenta uma tradução simples para verificar se está funcionando
                var result = await TranslatePortugueseToEnglishAsync("teste");
                return !string.IsNullOrWhiteSpace(result) && result != "teste";
            }
            catch
            {
                return false;
            }
        }
    }
}
