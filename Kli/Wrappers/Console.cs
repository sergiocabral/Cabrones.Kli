using System;
using System.Diagnostics.CodeAnalysis;

namespace Kli.Wrappers
{
    /// <summary>
    ///     Facade para System.Console.
    ///     Esta classe está fora do teste de cobertura porque alguns métodos
    ///     como ReadLine e ReadKey travam a execução do teste.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Console : IConsole
    {
        /// <summary>
        ///     Valor padrão para System.Console.ReadKey();
        /// </summary>
        private const bool ConsoleReadKeyInterceptMode = true;

        /// <summary>
        ///     For do texto.
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get => System.Console.ForegroundColor;
            set => System.Console.ForegroundColor = value;
        }

        /// <summary>
        ///     For do fundo do texto.
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get => System.Console.BackgroundColor;
            set => System.Console.BackgroundColor = value;
        }

        /// <summary>
        ///     Define as cores padrão no console.
        /// </summary>
        public void ResetColor()
        {
            System.Console.ResetColor();
        }

        /// <summary>
        ///     Escreve no console.
        /// </summary>
        /// <param name="value">Texto.</param>
        public void Write(string value)
        {
            System.Console.Write(value);
        }

        /// <summary>
        ///     Escreve no console com quebra de linha.
        /// </summary>
        /// <param name="value">Texto.</param>
        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }

        /// <summary>
        ///     Lê uma entrada de dados do usuário.
        /// </summary>
        /// <returns>Valor entrado.</returns>
        public string ReadLine()
        {
            return System.Console.ReadLine();
        }

        /// <summary>
        ///     Lê uma entrada de tecla de dados do usuário.
        /// </summary>
        /// <returns>Caracter digitado.</returns>
        public ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey(ConsoleReadKeyInterceptMode);
        }

        /// <summary>
        ///     Verifica se existe tecla disponível para leitura imediata.
        /// </summary>
        public bool KeyAvailable => System.Console.KeyAvailable;

        /// <summary>
        ///     Posição do cursor: topo
        /// </summary>
        public int CursorTop
        {
            get => System.Console.CursorTop;
            set => System.Console.CursorTop = value;
        }

        /// <summary>
        ///     Posição do cursor: esquerda
        /// </summary>
        public int CursorLeft
        {
            get => System.Console.CursorLeft;
            set => System.Console.CursorLeft = value;
        }

        /// <summary>
        ///     Comprimento do cursor: Altura
        /// </summary>
        public int BufferHeight => System.Console.BufferHeight;

        /// <summary>
        ///     Comprimento do cursor: largura
        /// </summary>
        public int BufferWidth => System.Console.BufferWidth;

        /// <summary>
        ///     Define a posição do cursor.
        /// </summary>
        /// <param name="left">Posição do cursor: esquerda</param>
        /// <param name="top">Posição do cursor: topo</param>
        public void SetCursorPosition(int left, int top)
        {
            System.Console.SetCursorPosition(left, top);
        }
    }
}