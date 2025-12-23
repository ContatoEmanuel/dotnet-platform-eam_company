namespace EAM.Web.Public.Helpers;

/// <summary>
/// Helper para trabalhar com idiomas na aplicação
/// </summary>
public static class LanguageHelper
{
    public const string DefaultLanguage = "pt-BR";
    public const string EnglishLanguage = "en-US";

    /// <summary>
    /// Obtém o idioma do cookie, query string ou default
    /// </summary>
    public static string GetLanguageFromRequest(HttpRequest request)
    {
        // Tenta obter do cookie primeiro
        if (request.Cookies.TryGetValue("eam_lang", out var cookieLanguage))
        {
            if (IsValidLanguage(cookieLanguage))
                return cookieLanguage;
        }

        // Tenta obter do query string
        if (request.Query.TryGetValue("lang", out var queryLanguage))
        {
            if (IsValidLanguage(queryLanguage))
                return queryLanguage;
        }

        // Retorna default
        return DefaultLanguage;
    }

    /// <summary>
    /// Valida se um idioma é suportado
    /// </summary>
    public static bool IsValidLanguage(string language)
    {
        return language == DefaultLanguage || language == EnglishLanguage;
    }

    /// <summary>
    /// Obtém o idioma normalizado (pt-BR ou en-US)
    /// </summary>
    public static string NormalizeLanguage(string language)
    {
        if (IsValidLanguage(language))
            return language;

        // Tenta normalizar
        if (language.StartsWith("pt"))
            return DefaultLanguage;
        if (language.StartsWith("en"))
            return EnglishLanguage;

        return DefaultLanguage;
    }
}
