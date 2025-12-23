using System.Net.Http.Json;
using EAM.Core.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EAM.Core.Application.Services
{
    /// <summary>
    /// Implementação de tradução automática usando Google Translate API.
    /// Usa o endpoint não-oficial mas estável do Google Translate.
    /// </summary>
    public class GoogleTranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GoogleTranslationService> _logger;
        private readonly string? _apiKey;
        private const string GoogleTranslateApiUrl = "https://translation.googleapis.com/language/translate/v2";

        public GoogleTranslationService(HttpClient httpClient, ILogger<GoogleTranslationService> logger, string? apiKey = null)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = apiKey;
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
                _logger.LogError(ex, "Erro ao traduzir texto: {Text}", textToTranslate);
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

                // Google Translate tem limite de caracteres por requisição
                // Vamos processar em chunks se necessário
                var chunks = ChunkTexts(textsToTranslate, 5000);

                foreach (var chunk in chunks)
                {
                    var translated = await TranslateChunkAsync(chunk);
                    results.AddRange(translated);
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao traduzir lote de {Count} textos", textsToTranslate.Count);
                return textsToTranslate; // Retorna originals em caso de erro
            }
        }

        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                // Tenta uma tradução simples para verificar se está funcionando
                var result = await TranslatePortugueseToEnglishAsync("teste");
                return !string.IsNullOrWhiteSpace(result);
            }
            catch
            {
                return false;
            }
        }

        private async Task<List<string>> TranslateChunkAsync(List<string> texts)
        {
            var request = new
            {
                q = texts,
                source = "pt",
                target = "en",
                format = "html" // Preserva HTML tags
            };

            var uri = _apiKey != null 
                ? $"{GoogleTranslateApiUrl}?key={_apiKey}" 
                : GoogleTranslateApiUrl;

            var response = await _httpClient.PostAsJsonAsync(uri, request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsAsync<dynamic>();
            var translations = new List<string>();

            foreach (var data in jsonResponse["data"]["translations"])
            {
                translations.Add(data["translatedText"].ToString());
            }

            return translations;
        }

        private List<List<string>> ChunkTexts(List<string> texts, int maxCharacters)
        {
            var chunks = new List<List<string>>();
            var currentChunk = new List<string>();
            var currentSize = 0;

            foreach (var text in texts)
            {
                if (currentSize + text.Length > maxCharacters && currentChunk.Count > 0)
                {
                    chunks.Add(currentChunk);
                    currentChunk = new List<string>();
                    currentSize = 0;
                }

                currentChunk.Add(text);
                currentSize += text.Length;
            }

            if (currentChunk.Count > 0)
                chunks.Add(currentChunk);

            return chunks;
        }
    }
}
