using System;

namespace Kli.IO
{
    /// <summary>
    /// Interface para classe capaz de escrever o texto com a devida formatação.
    /// </summary>
    public interface IOutputWriter
    {
        /// <summary>
        /// Analisa um texto com marcadores e envia os trechos para cada marcador para escrita.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="writer">Recebe o texto para escrita. Informa o marcador a ser aplicado.</param>
        void Parse(string text, Action<string, char> writer);

        /// <summary>
        /// Analisa um texto com marcadores e envia o texto inteiro desconsiderando os marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="writer">Recebe o texto para escrita.</param>
        void Parse(string text, Action<string> writer);
    }
}