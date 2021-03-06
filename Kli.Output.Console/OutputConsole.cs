﻿using System;
using Kli.Wrappers;

namespace Kli.Output.Console
{
    /// <summary>
    ///     Envia dados para o usuário via console.
    /// </summary>
    public class OutputConsole : IOutputConsole
    {
        /// <summary>
        ///     Define as cores padrão no console.
        /// </summary>
        private readonly IConsole _console;

        /// <summary>
        ///     Converte marcadores para cores tipo ConsoleColor.
        /// </summary>
        private readonly IOutputMarkersToConsoleColor _outputMarkersToConsoleColor;

        /// <summary>
        ///     Capaz de escrever o texto com a devida formatação.
        /// </summary>
        private readonly IOutputWriter _outputWriter;

        /// <summary>
        ///     Construtor.
        /// </summary>
        /// <param name="outputWriter">Capaz de escrever o texto com a devida formatação.</param>
        /// <param name="outputMarkersToConsoleColorToConsoleColor">Converte marcadores para cores tipo ConsoleColor.</param>
        /// <param name="console">Define as cores padrão no console.</param>
        public OutputConsole(
            IOutputWriter outputWriter,
            IOutputMarkersToConsoleColor outputMarkersToConsoleColorToConsoleColor,
            IConsole console)
        {
            _outputWriter = outputWriter;
            _outputMarkersToConsoleColor = outputMarkersToConsoleColorToConsoleColor;
            _console = console;
        }

        /// <summary>
        ///     Escreve um texto para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput Write(string text)
        {
            _outputWriter.Parse(text, WriteToConsole);
            return this;
        }

        /// <summary>
        ///     Escreve um texto (com quebra de linha) para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput WriteLine(string text)
        {
            return Write($"{text}\n");
        }

        /// <summary>
        ///     Escritor para o usuário no console.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="marker">Marcador aplicado ao texto.</param>
        public void WriteToConsole(string text, char marker = (char) 0)
        {
            var (foreground, background) = _outputMarkersToConsoleColor.Convert(marker);

            _console.BackgroundColor = background;
            _console.ForegroundColor = foreground;

            _console.Write(text);
        }

        /// <summary>
        ///     Retorna o marcador com base nas cores atuais do console.
        /// </summary>
        /// <returns>Marcador.</returns>
        public char CurrentMarker()
        {
            return _outputMarkersToConsoleColor.Convert(_console.ForegroundColor, _console.BackgroundColor);
        }
    }
}