using System;
using Kli.IO;

namespace Kli.Output.Console
{
    /// <summary>
    /// Envia dados para o usuário via console.
    /// </summary>
    public class OutputConsole: IOutputConsole
    {
        /// <summary>
        /// Capaz de escrever o texto com a devida formatação.
        /// </summary>
        private readonly IOutputWriter _outputWriter;

        /// <summary>
        /// Converte marcadores para cores tipo ConsoleColor.
        /// </summary>
        private readonly IOutputMarkersToConsoleColor _outputMarkersToConsoleColor;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="outputWriter">Capaz de escrever o texto com a devida formatação.</param>
        /// <param name="outputMarkersToConsoleColorToConsoleColor">Converte marcadores para cores tipo ConsoleColor.</param>
        public OutputConsole(IOutputWriter outputWriter, IOutputMarkersToConsoleColor outputMarkersToConsoleColorToConsoleColor)
        {
            _outputWriter = outputWriter;
            _outputMarkersToConsoleColor = outputMarkersToConsoleColorToConsoleColor;
        }
        
        /// <summary>
        /// Escreve um texto para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput Write(string text)
        {
            _outputWriter.Parse(text, WriteToConsole);
            return this;
        }

        /// <summary>
        /// Escreve um texto (com quebra de linha) para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput WriteLine(string text) => Write($"{text}\n");

        /// <summary>
        /// Escritor para o usuário no console.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="marker">Marcador aplicado ao texto.</param>
        public void WriteToConsole(string text, char marker = (char) 0)
        {
            var (foreground, background) = _outputMarkersToConsoleColor.Convert(marker);
            System.Console.BackgroundColor = background;
            System.Console.ForegroundColor = foreground;

            System.Console.Write(text);
        }
    }
}