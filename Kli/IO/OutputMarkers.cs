using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    /// <summary>
    /// Marcadores para texto de saída para o usuário.
    /// </summary>
    public class OutputMarkers : IOutputMarkers
    {
        /// <summary>
        /// Marcador de: erro
        /// </summary>
        public char Error { get; } = '!';
        
        /// <summary>
        /// Marcador de: pergunta
        /// </summary>
        public char Question { get; } = '?';
        
        /// <summary>
        /// Marcador de: resposta
        /// </summary>
        public char Answer { get; } = '@';
        
        /// <summary>
        /// Marcador de: alto destaque
        /// </summary>
        public char Highlight { get; } = '*';
        
        /// <summary>
        /// Marcador de: destaque menor
        /// </summary>
        public char Light { get; } = '_';
        
        /// <summary>
        /// Marcador de: baixa importância
        /// </summary>
        public char Low { get; } = '#';

        /// <summary>
        /// Cache para propriedade Markers
        /// </summary>
        private static char[] _chars = { };
        
        /// <summary>
        /// Lista de todos os caracteres especiais.
        /// </summary>
        public IEnumerable<char> Markers
        {
            get
            {
                if (_chars.Length > 0) return _chars;

                _chars = (
                    from property in GetType().GetProperties()
                    where property.PropertyType == typeof(char)
                    select (char) (property.GetValue(this) ?? 0)
                    into ch
                    where ch != (char) 0
                    select ch
                ).ToArray();

                return _chars;
            }
        }

        /// <summary>
        /// Escapa o texto para escrever no output mesmo os caracteres de marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto devidamente escapado.</returns>
        public string Escape(string text)
        {
            var result = new StringBuilder(text);
            foreach (var item in Markers)
            {
                result.Replace(item.ToString(), item + item.ToString());
            }
            return result.ToString();
        }
    }
}