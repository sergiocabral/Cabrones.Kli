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
}