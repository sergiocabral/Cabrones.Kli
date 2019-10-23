using System;

namespace Kli.Wrappers
{
    /// <summary>
    /// Facade para System.Console.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// For do texto.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// For do fundo do texto.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Define as cores padrão no console.
        /// </summary>
        void ResetColor();
        
        /// <summary>
        /// Escreve no console. 
        /// </summary>
        /// <param name="value">Texto.</param>
        void Write(string value);
        
        /// <summary>
        /// Escreve no console com quebra de linha. 
        /// </summary>
        /// <param name="value">Texto.</param>
        void WriteLine(string value);
    }
}