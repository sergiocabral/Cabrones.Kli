using System.Collections.Generic;

namespace Kli.IO
{
    /// <summary>
    /// Marcadores para texto de saída para o usuário.
    /// </summary>
    public interface IOutputMarkers
    {
        /// <summary>
        /// Marcador de: erro
        /// </summary>
        char Error { get; }
        
        /// <summary>
        /// Marcador de: pergunta
        /// </summary>
        char Question { get; }
        
        /// <summary>
        /// Marcador de: resposta
        /// </summary>
        char Answer { get; }
        
        /// <summary>
        /// Marcador de: alto destaque
        /// </summary>
        char Highlight { get; }
        
        /// <summary>
        /// Marcador de: destaque menor
        /// </summary>
        char Light { get; }
        
        /// <summary>
        /// Marcador de: baixa importância
        /// </summary>
        char Low { get; }

        /// <summary>
        /// Lista de todos os caracteres especiais.
        /// </summary>
        IEnumerable<char> Markers { get; }

        /// <summary>
        /// Escapa o texto para escrever no output mesmo os caracteres de marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto devidamente escapado.</returns>
        string Escape(string text);
    }
}