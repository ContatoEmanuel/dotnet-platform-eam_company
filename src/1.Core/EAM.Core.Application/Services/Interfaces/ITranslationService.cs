namespace EAM.Core.Application.Services.Interfaces
{
    /// <summary>
    /// Interface para serviços de tradução automática.
    /// Abstrai a implementação específica de cada provedor (Google, Azure, DeepL, etc).
    /// </summary>
    public interface ITranslationService
    {
        /// <summary>
        /// Traduz um texto do português para inglês.
        /// </summary>
        /// <param name="textToTranslate">Texto a ser traduzido</param>
        /// <returns>Texto traduzido em inglês</returns>
        Task<string> TranslatePortugueseToEnglishAsync(string textToTranslate);

        /// <summary>
        /// Traduz múltiplos textos de uma vez.
        /// </summary>
        /// <param name="textsToTranslate">Lista de textos</param>
        /// <returns>Lista de textos traduzidos</returns>
        Task<List<string>> TranslatePortugueseToEnglishBatchAsync(List<string> textsToTranslate);

        /// <summary>
        /// Verifica se o serviço está configurado e disponível.
        /// </summary>
        /// <returns>true se pode fazer requisições, false caso contrário</returns>
        Task<bool> IsAvailableAsync();
    }
}
