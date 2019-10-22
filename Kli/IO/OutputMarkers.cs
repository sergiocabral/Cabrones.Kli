using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        /// Cache para propriedade em geral.
        /// </summary>
        private static readonly IDictionary<string, object> Cache = new Dictionary<string, object>();

        /// <summary>
        /// Chave identificador do valor de cache para a propriedade: Markers
        /// </summary>
        private const string CacheKeyMarkers = "Markers";
        
        /// <summary>
        /// Lista de todos os caracteres especiais.
        /// </summary>
        public string Markers
        {
            get
            {
                if (Cache.ContainsKey(CacheKeyMarkers)) 
                    return (string)Cache[CacheKeyMarkers];

                return (string) (Cache[CacheKeyMarkers] = string.Join("", (
                    from property in GetType().GetProperties()
                    where property.PropertyType == typeof(char)
                    select (char) (property.GetValue(this) ?? 0)
                    into ch
                    where ch != (char) 0
                    select ch
                ).ToArray()));
            }
        }

        /// <summary>
        /// Chave identificador do valor de cache para a propriedade: Markers
        /// </summary>
        private const string CacheKeyMarkersEscapedForRegexJoined = "MarkersEscapedForRegexJoined";
        
        /// <summary>
        /// Lista de marcadores devidamente escapados para Regex.
        /// </summary>
        public string MarkersEscapedForRegexJoined
        {
            get
            {
                if (Cache.ContainsKey(CacheKeyMarkersEscapedForRegexJoined))
                    return (string) Cache[CacheKeyMarkersEscapedForRegexJoined];

                return (string) (Cache[CacheKeyMarkersEscapedForRegexJoined] = 
                    string.Join("", MarkersEscapedForRegexSeparated));
            }
        }

        /// <summary>
        /// Chave identificador do valor de cache para a propriedade: Markers
        /// </summary>
        private const string CacheKeyMarkersEscapedForRegexSeparated = "MarkersEscapedForRegexSeparated";
        
        /// <summary>
        /// Lista de marcadores devidamente escapados para Regex.
        /// </summary>
        public string[] MarkersEscapedForRegexSeparated
        {
            get
            {
                if (Cache.ContainsKey(CacheKeyMarkersEscapedForRegexSeparated))
                    return (string[]) Cache[CacheKeyMarkersEscapedForRegexSeparated];

                return (string[]) (Cache[CacheKeyMarkersEscapedForRegexSeparated] =
                    Markers.Select(marker => Regex.Escape(marker.ToString())).ToArray());
            }
        }

        /// <summary>
        /// Escapa o texto para escrever no output mesmo os caracteres de marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto devidamente escapado.</returns>
        public string Escape(string text)
        {
            if (string.IsNullOrWhiteSpace(text) ||
                !Regex.IsMatch(text, $"[{MarkersEscapedForRegexJoined}]", RegexOptions.Singleline)) 
                return text;
            
            var result = new StringBuilder(text);
            foreach (var item in Markers)
            {
                result.Replace(item.ToString(), item + item.ToString());
            }
            return result.ToString();
        }
    }
}