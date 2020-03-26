using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kli.Infrastructure;

namespace Kli.Output
{
    /// <summary>
    ///     Marcadores para texto de saída para o usuário.
    /// </summary>
    public class OutputMarkers : IOutputMarkers
    {
        /// <summary>
        ///     Chave identificador do valor de cache para a propriedade: Markers
        /// </summary>
        private const string CacheKeyMarkers = "OutputMarkers.OutputMarkersMarkers";

        /// <summary>
        ///     Chave identificador do valor de cache para a propriedade: Markers
        /// </summary>
        private const string CacheKeyMarkersEscapedForRegexJoined = "OutputMarkers.MarkersEscapedForRegexJoined";

        /// <summary>
        ///     Chave identificador do valor de cache para a propriedade: Markers
        /// </summary>
        private const string CacheKeyMarkersEscapedForRegexSeparated = "OutputMarkers.MarkersEscapedForRegexSeparated";

        /// <summary>
        ///     Cache simples para valores.
        /// </summary>
        private readonly ICache _cache;

        /// <summary>
        ///     Construtor.
        /// </summary>
        /// <param name="cache">Cache simples para valores.</param>
        public OutputMarkers(ICache cache)
        {
            _cache = cache;
        }

        /// <summary>
        ///     Marcador de: erro
        /// </summary>
        public char Error { get; } = '!';

        /// <summary>
        ///     Marcador de: pergunta
        /// </summary>
        public char Question { get; } = '?';

        /// <summary>
        ///     Marcador de: resposta
        /// </summary>
        public char Answer { get; } = '@';

        /// <summary>
        ///     Marcador de: alto destaque
        /// </summary>
        public char Highlight { get; } = '*';

        /// <summary>
        ///     Marcador de: destaque menor
        /// </summary>
        public char Light { get; } = '_';

        /// <summary>
        ///     Marcador de: baixa importância
        /// </summary>
        public char Low { get; } = '#';

        /// <summary>
        ///     Lista de todos os caracteres especiais.
        /// </summary>
        public string Markers =>
            _cache.Get<string>(CacheKeyMarkers) ??
            _cache.Set(CacheKeyMarkers,
                new string((
                    from property in GetType().GetProperties()
                    where property.PropertyType == typeof(char)
                    select (char) (property.GetValue(this) ?? 0)
                    into ch
                    where ch != (char) 0
                    select ch
                ).ToArray()));

        /// <summary>
        ///     Lista de marcadores devidamente escapados para Regex.
        /// </summary>
        public string MarkersEscapedForRegexJoined =>
            _cache.Get<string>(CacheKeyMarkersEscapedForRegexJoined) ??
            _cache.Set(CacheKeyMarkersEscapedForRegexJoined,
                string.Join(string.Empty, MarkersEscapedForRegexSeparated));

        /// <summary>
        ///     Lista de marcadores devidamente escapados para Regex.
        /// </summary>
        public string[] MarkersEscapedForRegexSeparated =>
            _cache.Get<string[]>(CacheKeyMarkersEscapedForRegexSeparated) ??
            _cache.Set(CacheKeyMarkersEscapedForRegexSeparated,
                Markers.Select(marker => Regex.Escape(marker.ToString())).ToArray());

        /// <summary>
        ///     Escapa o texto para escrever no output mesmo os caracteres de marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto devidamente escapado.</returns>
        public string Escape(string text)
        {
            if (string.IsNullOrWhiteSpace(text) ||
                !Regex.IsMatch(text, $"[{MarkersEscapedForRegexJoined}]", RegexOptions.Singleline))
                return text;

            var result = new StringBuilder(text);
            foreach (var item in Markers) result.Replace(item.ToString(), item + item.ToString());

            return result.ToString();
        }
    }
}