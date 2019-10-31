using System;

namespace Kli.Wrappers
{
    /// <summary>
    /// Facade para System.Console.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Cor do texto.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Cor do fundo do texto.
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
        
        /// <summary>
        /// Lê uma entrada de dados do usuário.
        /// </summary>
        /// <returns>Valor entrado.</returns>
        string ReadLine();

        /// <summary>
        /// Lê uma entrada de tecla de dados do usuário.
        /// </summary>
        /// <returns>Caracter digitado.</returns>
        ConsoleKeyInfo ReadKey();

        /// <summary>
        /// Verifica se existe tecla disponível para leitura imediata.
        /// </summary>
        bool KeyAvailable { get; }

        /// <summary>
        /// Posição do cursor: topo
        /// </summary>
        int CursorTop { get; set; }
        
        /// <summary>
        /// Posição do cursor: esquerda
        /// </summary>
        int CursorLeft { get; set; }
        
        /// <summary>
        /// Comprimento do cursor: Altura
        /// </summary>
        int BufferHeight { get; }
        
        /// <summary>
        /// Comprimento do cursor: largura
        /// </summary>
        int BufferWidth { get; }

        /// <summary>
        /// Define a posição do cursor.
        /// </summary>
        /// <param name="left">Posição do cursor: esquerda</param>
        /// <param name="top">Posição do cursor: topo</param>
        void SetCursorPosition(int left, int top);
    }
}