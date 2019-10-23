using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Kli.Infrastructure;
using Kli.Wrappers;

namespace Kli.i18n
{
    public class Language: ILanguage
    {
        /// <summary>
        /// Lista de nomes de variáveis de ambiente que serão consultadas para obter o idioma
        /// </summary>
        public IEnumerable<string> EnvironmentVariables { get; } = new[]
        {
            "KLI-LANG",
            "KLI_LANG"
        };
        
        /// <summary>
        /// Cache simples para valores.
        /// </summary>
        private readonly ICache _cache;

        /// <summary>
        /// Facade para System.Environment.
        /// </summary>
        private readonly IEnvironment _environment;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="cache">Cache simples para valores.</param>
        /// <param name="environment">Facade para System.Environment.</param>
        public Language(ICache cache, IEnvironment environment)
        {
            _cache = cache;
            _environment = environment;
        }
        
        /// <summary>
        /// Idioma obtido do ambiente.
        /// </summary>
        public string FromEnvironment() =>
            NormalizeLanguageName(
                EnvironmentVariables.Select(environmentVariable => 
                    _environment.GetEnvironmentVariable(environmentVariable))
                    .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)));

        /// <summary>
        /// Idioma obtido do sistema.
        /// </summary>
        public string FromSystem() => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        
        /// <summary>
        /// Chave identificador do valor de cache para a propriedade: Current
        /// </summary>
        private const string CacheKeyCurrent = "Culture.Current";

        /// <summary>
        /// Idioma atual definido pela variável de ambiente ou pelo sistema operacional.
        /// </summary>
        public string Current =>
            _cache.Get<string>(CacheKeyCurrent) ??
            _cache.Set(CacheKeyCurrent, FromEnvironment() ?? FromSystem());

        /// <summary>
        /// Normaliza o nome da cultura para um valor válido.
        /// </summary>
        /// <param name="culture">Nome.</param>
        /// <returns>Nome válido.</returns>
        private static string NormalizeLanguageName(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture)) return null!;
            
            try
            {
                return new CultureInfo(culture).TwoLetterISOLanguageName;
            }
            catch
            {
                return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }
        }

    }
}