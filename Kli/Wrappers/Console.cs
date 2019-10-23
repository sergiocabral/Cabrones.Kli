using System;

namespace Kli.Wrappers
{
    /// <summary>
    /// Facade para System.Console.
    /// </summary>
    public class Console: IConsole
    {
        /// <summary>
        /// For do texto.
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get => System.Console.ForegroundColor; 
            set => System.Console.ForegroundColor = value;
        }

        /// <summary>
        /// For do fundo do texto.
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get => System.Console.BackgroundColor; 
            set => System.Console.BackgroundColor = value;
        }
        
        /// <summary>
        /// Define as cores padrão no console.
        /// </summary>
        public void ResetColor() => System.Console.ResetColor();
        
        /// <summary>
        /// Escreve no console. 
        /// </summary>
        /// <param name="value">Texto.</param>
        public void Write(string value) => System.Console.Write(value);
        
        /// <summary>
        /// Escreve no console com quebra de linha. 
        /// </summary>
        /// <param name="value">Texto.</param>
        public void WriteLine(string value) => System.Console.WriteLine(value);
    }
}