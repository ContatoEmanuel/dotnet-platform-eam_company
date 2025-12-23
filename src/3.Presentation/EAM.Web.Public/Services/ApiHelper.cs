using EAM.Web.Public.Helpers;

namespace EAM.Web.Public.Services;

/// <summary>
/// Helper para requisições HTTP com suporte a idioma
/// </summary>
public static class ApiHelper
{
    /// <summary>
    /// Adiciona o parâmetro de idioma à URL
    /// </summary>
    public static string AddLanguageParameter(string url, string language)
    {
        var separator = url.Contains("?") ? "&" : "?";
        return $"{url}{separator}language={language}";
    }

    /// <summary>
    /// Obtém a URL da API com o parâmetro de idioma
    /// </summary>
    public static string GetApiUrlWithLanguage(string baseUrl, string language)
    {
        if (string.IsNullOrEmpty(language) || !LanguageHelper.IsValidLanguage(language))
            language = LanguageHelper.DefaultLanguage;

        return AddLanguageParameter(baseUrl, language);
    }
}
