using System;

namespace Kli.Output
{
    /// <summary>
    ///     Converte marcadores para cores tipo ConsoleColor.
    /// </summary>
    public interface IOutputMarkersToConsoleColor
    {
        /// <summary>
        ///     Converte um marcador em cor.
        /// </summary>
        /// <param name="marker">Marcador.</param>
        /// <returns>Cores foreground e background.</returns>
        Tuple<ConsoleColor, ConsoleColor> Convert(char marker);

        /// <summary>
        ///     Converte uma cor em marcador.
        /// </summary>
        /// <param name="foreground">Cor de foreground.</param>
        /// <param name="background">Cor de background.</param>
        /// <returns>Marcador.</returns>
        char Convert(ConsoleColor foreground, ConsoleColor background = ConsoleColor.Black);
    }
}