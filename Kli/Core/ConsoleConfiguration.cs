using System;

namespace Kli.Core
{
    /// <summary>
    /// Configurações relacionadas ao console.
    /// </summary>
    public interface IConsoleConfiguration
    {
        /// <summary>
        /// Backup de valor para: Console.BackgroundColor
        /// </summary>
        ConsoleColor BackgroundColorBackup { get; }

        /// <summary>
        /// Backup de valor para: Console.ForegroundColor
        /// </summary>
        ConsoleColor ForegroundColorBackup { get; }
        
        /// <summary>
        /// Grava as cores atuais para poder restaurar depois.
        /// </summary>
        void SaveCurrentColor();
        
        /// <summary>
        /// Restaura as cores salvas pela última vez.
        /// </summary>
        void RestoreColor();
        
        /// <summary>
        /// Define as cores padrão para funcionamento do programa.
        /// </summary>
        void SetDefaultColor();
    }

    /// <summary>
    /// Configurações relacionadas ao console.
    /// </summary>
    public class ConsoleConfiguration : IConsoleConfiguration
    {
        /// <summary>
        /// Valor padrão para: Console.BackgroundColor
        /// </summary>
        public const ConsoleColor BackgroundColorDefault = ConsoleColor.Black;

        /// <summary>
        /// Valor padrão para: Console.ForegroundColor
        /// </summary>
        public const ConsoleColor ForegroundColorDefault = ConsoleColor.Gray;

        /// <summary>
        /// Backup de valor para: Console.BackgroundColor
        /// </summary>
        public ConsoleColor BackgroundColorBackup { get; private set; } = Console.BackgroundColor;

        /// <summary>
        /// Backup de valor para: Console.ForegroundColor
        /// </summary>
        public ConsoleColor ForegroundColorBackup { get; private set; }= Console.ForegroundColor;

        /// <summary>
        /// Grava as cores atuais para poder restaurar depois.
        /// </summary>
        public void SaveCurrentColor()
        {
            BackgroundColorBackup = Console.BackgroundColor;
            ForegroundColorBackup = Console.ForegroundColor;
        }

        /// <summary>
        /// Restaura as cores salvas pela última vez.
        /// </summary>
        public void RestoreColor()
        {
            Console.BackgroundColor = BackgroundColorBackup;
            Console.ForegroundColor = ForegroundColorBackup;
        }

        /// <summary>
        /// Define as cores padrão para funcionamento do programa.
        /// </summary>
        public void SetDefaultColor()
        {
            Console.BackgroundColor = BackgroundColorDefault;
            Console.ForegroundColor = ForegroundColorDefault;
        }
    }
}