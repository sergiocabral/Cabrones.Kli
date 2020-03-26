using System.Collections.Generic;
using System.Reflection;

namespace Kli.i18n
{
    /// <summary>
    ///     Manipula traduções de texto.
    /// </summary>
    public interface ITranslate
    {
        /// <summary>
        ///     Idioma padrão.
        /// </summary>
        string LanguageDefault { get; set; }

        /// <summary>
        ///     Lista de traduções atuais.
        /// </summary>
        IDictionary<string, IDictionary<string, string>> Translates { get; }

        /// <summary>
        ///     Retorna a tradução de um texto.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="language">Idioma.</param>
        /// <returns>Tradução.</returns>
        string Get(string text, string? language = null);

        /// <summary>
        ///     Limpa todas as traduções carregadas.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Carregar as traduções.
        /// </summary>
        /// <param name="source">Dicionário com traduções.</param>
        /// <returns>Traduções carregadas.</returns>
        IDictionary<string, IDictionary<string, string>>? LoadFromDictionary(
            IDictionary<string, IDictionary<string, string>> source);

        /// <summary>
        ///     Carregar as traduções.
        /// </summary>
        /// <param name="source">Texto com traduções.</param>
        /// <returns>Traduções carregadas.</returns>
        IDictionary<string, IDictionary<string, string>>? LoadFromText(string source);

        /// <summary>
        ///     Carregar as traduções.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resource">Nome do recurso.</param>
        /// <returns>Traduções carregadas.</returns>
        IDictionary<string, IDictionary<string, string>>? LoadFromResource(Assembly assembly, string resource);
    }
}