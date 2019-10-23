using System;
using System.Linq;
using Kli.IO;

namespace Kli.Output.Console
{
    /// <summary>
    /// Converte marcadores para cores tipo ConsoleColor.
    /// </summary>
    public class OutputMarkersToConsoleColor: IOutputMarkersToConsoleColor
    {
        /// <summary>
        /// Definições de cores.
        /// </summary>
        private readonly Tuple<char, ConsoleColor, ConsoleColor>[] _colors;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="outputMarkers">Marcadores para texto de saída para o usuário.</param>
        public OutputMarkersToConsoleColor(IOutputMarkers outputMarkers)
        {
            _colors = new[]
            {
                new Tuple<char, ConsoleColor, ConsoleColor>((char) 0, ConsoleColor.Gray, ConsoleColor.Black),
                new Tuple<char, ConsoleColor, ConsoleColor>(outputMarkers.Low, ConsoleColor.DarkGray, ConsoleColor.Black),
                new Tuple<char, ConsoleColor, ConsoleColor>(outputMarkers.Light, ConsoleColor.Magenta, ConsoleColor.Black),
                new Tuple<char, ConsoleColor, ConsoleColor>(outputMarkers.Highlight, ConsoleColor.DarkCyan, ConsoleColor.Black),
                new Tuple<char, ConsoleColor, ConsoleColor>(outputMarkers.Question, ConsoleColor.DarkYellow, ConsoleColor.Black),
                new Tuple<char, ConsoleColor, ConsoleColor>(outputMarkers.Answer, ConsoleColor.Yellow, ConsoleColor.Black),
                new Tuple<char, ConsoleColor, ConsoleColor>(outputMarkers.Error, ConsoleColor.Red, ConsoleColor.Black)
            };
        }

        /// <summary>
        /// Converte um marcador em cor.
        /// </summary>
        /// <param name="marker">Marcador.</param>
        /// <returns>Cores foreground e background.</returns>
        public Tuple<ConsoleColor, ConsoleColor> Convert(char marker)
        {
            var result = _colors.FirstOrDefault(a => a.Item1 == marker);
            return result != null
                ? new Tuple<ConsoleColor, ConsoleColor>(result.Item2, result.Item3)
                : new Tuple<ConsoleColor, ConsoleColor>(_colors[0].Item2, _colors[0].Item3);
        }

        /// <summary>
        /// Converte uma cor em marcador.
        /// </summary>
        /// <param name="foreground">Cor de foreground.</param>
        /// <param name="background">Cor de background.</param>
        /// <returns>Marcador.</returns>
        public char Convert(ConsoleColor foreground, ConsoleColor background = ConsoleColor.Black)
        {
            var result = _colors.FirstOrDefault(a => a.Item2 == foreground && a.Item3 == background);
            return result?.Item1 ?? _colors[0].Item1;
        }
    }
}