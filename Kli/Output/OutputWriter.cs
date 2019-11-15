using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kli.Output
{
    /// <summary>
    /// Classe capaz de escrever o texto com a devida formatação.
    /// </summary>
    public class OutputWriter : IOutputWriter
    {
        /// <summary>
        /// Marcadores para texto de saída para o usuário.
        /// </summary>
        private readonly IOutputMarkers _outputMarkers;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="outputMarkers">Marcadores para texto de saída para o usuário.</param>
        public OutputWriter(IOutputMarkers outputMarkers)
        {
            _outputMarkers = outputMarkers;
        }

        /// <summary>
        /// Analisa um texto com marcadores e envia os trechos para cada marcador para escrita.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="writer">Recebe o texto para escrita. Informa o marcador a ser aplicado.</param>
        public void Parse(string text, Action<string, char> writer)
        {
            if (string.IsNullOrEmpty(text)) return;
            
            text = NormalizeNewLine(text);
            foreach (var (part, mark) in ExtractParts(text))
            {
                if (!string.IsNullOrEmpty(text)) writer(part, mark);
            }
        }

        /// <summary>
        /// Analisa um texto com marcadores e envia o texto inteiro desconsiderando os marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="writer">Recebe o texto para escrita.</param>
        public void Parse(string text, Action<string> writer)
        {
            if (string.IsNullOrEmpty(text)) return;
            
            text = NormalizeNewLine(text);
            text = RemoveAllMarkers(text);
            
            if (!string.IsNullOrEmpty(text)) writer(text);
        }

        /// <summary>
        /// Extrai as partes do textos com seu respetivo marcador.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Lista de partes do texto.</returns>
        private IEnumerable<Tuple<string, char>> ExtractParts(string text)
        {
            var parts = new List<Tuple<string, char>>();

            // Posições onde há marcadores.
            var indexes = Regex.Matches(text, $"[{_outputMarkers.MarkersEscapedForRegexJoined}]", RegexOptions.Singleline).Select(a => a.Index).ToList();

            if (indexes.Count == 0)
            {
                // O texto não possui marcadores. Então, envia tudo com marcador nulo.
                parts.Add(new Tuple<string, char>(text, (char) 0));
            }
            else
            {
                // Marcadores em aberto.
                var markers = new List<char> {(char) 0};

                // Parte do texto atualmente em análise. 
                var part = new StringBuilder(text.Substring(0, indexes[0]));

                var position = indexes[0];
                while (position < text.Length)
                {
                    if (position < text.Length - 1 && text[position] == text[position + 1])
                    {
                        position++;
                        indexes.RemoveAt(0);
                        indexes.RemoveAt(0);
                    }
                    else
                    {
                        if (part.Length > 0) parts.Add(new Tuple<string, char>(part.ToString(), markers[0]));

                        part.Clear();
                        if (markers[0] == text[position])
                        {
                            // Finaliza marcador em aberto.
                            markers.RemoveAt(0);
                        }
                        else
                        {
                            // Abre novo marcador.
                            markers.Insert(0, text[position]);
                        }

                        position++;
                        indexes.RemoveAt(0);
                    }

                    var index = indexes.Count > 0 ? indexes[0] : text.Length;
                    part.Append(text.Substring(position, index - position));
                    position = index;
                }
                
                if (part.Length > 0) parts.Add(new Tuple<string, char>(part.ToString(), markers[0]));
            }

            return parts;
        }
        
        /// <summary>
        /// Remove todos os marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto sem marcadores</returns>
        private string RemoveAllMarkers(string text)
        {
            // Não possui marcadores, então sai.
            if (!Regex.IsMatch(text, $@"[{_outputMarkers.MarkersEscapedForRegexJoined}]", RegexOptions.Singleline)) return text;

            for (var i = 0; i < _outputMarkers.Markers.Length; i++)
            {
                var marker = _outputMarkers.Markers[i];

                // Não tem esse marcador, passa para o próximo.
                if (!text.Contains(marker)) continue;

                var markerEscaped = _outputMarkers.MarkersEscapedForRegexSeparated[i];

                // Remove marcador simples como _aqui_ dessa forma.
                text = Regex.Replace(text, $@"(?<!{markerEscaped})({markerEscaped})(?!{markerEscaped})", string.Empty, RegexOptions.Singleline);

                // Não tem mais esse marcador, passa para o próximo.
                if (!text.Contains(marker)) continue;
                
                // Agora faz análise mediante varredura.

                // Adiciona um caracter extra para evitar exception de Index Out Of Bound.
                text += (char)0;
                
                var position = text.IndexOf(marker);
                while (position >= 0)
                {
                    if (text[position + 1] == marker)
                    {
                        position++;
                    }
                    text = text.Remove(position, 1);
                    position = text.IndexOf(marker, position);
                };

                // Remove o caracter adicional.
                text = text.Substring(0, text.Length - 1);
            }
            
            return text;
        }
        
        /// <summary>
        /// Normaliza nova linha para o formato do sistema operacional corrente.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto normalizado.</returns>
        private static string NormalizeNewLine(string text)
        {
            return text
                .Replace("\r\n", "\n")
                .Replace("\n\r", "\n")
                .Replace("\r", "\n")
                .Replace("\n", Environment.NewLine);
        }
    }
}