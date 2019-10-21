using System.Linq;
using System.Text;

namespace Kli.IO
{
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
        private static string _markers = string.Empty;
        
        /// <summary>
        /// Lista de todos os caracteres especiais.
        /// </summary>
        public string Markers
        {
            get
            {
                if (_markers.Length > 0) return _markers;

                _markers = string.Join("", (
                    from property in GetType().GetProperties()
                    where property.PropertyType == typeof(char)
                    select (char) (property.GetValue(this) ?? 0)
                    into ch
                    where ch != (char) 0
                    select ch
                ).ToArray());

                return _markers;
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