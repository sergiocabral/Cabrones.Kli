using System.Collections.Generic;

namespace Kli.i18n
{
    public interface ILanguage
    {
        /// <summary>
        /// Lista de nomes de variáveis de ambiente que serão consultadas para obter o idioma.
        /// </summary>
        IEnumerable<string> EnvironmentVariables { get; }

        /// <summary>
        /// Idioma obtido do ambiente.
        /// </summary>
        string FromEnvironment();

        /// <summary>
        /// Idioma obtido do sistema.
        /// </summary>
        string FromSystem();
        
        /// <summary>
        /// Idioma atual definido pela variável de ambiente ou pelo sistema operacional.
        /// </summary>
        string Current { get; }
    }
}