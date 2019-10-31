using System;
using System.Diagnostics.CodeAnalysis;
using Kli.Wrappers;
using Environment = System.Environment;

namespace Kli.Input.Console
{
    /// <summary>
    /// Classe implementada apenas para simular um console.
    /// Pode ter partes ignoradas durante o teste de cobertura.
    /// </summary>
    internal class SimulationForIConsole : IConsole
    {
        /// <summary>
        /// Resposta a ser emitida pelos métodos ReadLine() ou Read().
        /// </summary>
        private string _resposta;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="resposta">Resposta a ser emitida pelos métodos ReadLine() ou Read().</param>
        public SimulationForIConsole(string resposta)
        {
            _resposta = $"{resposta}";
        }

        /// <summary>
        /// Cor do texto.
        /// </summary>
        [ExcludeFromCodeCoverage] public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Cor do fundo do texto.
        /// </summary>
        [ExcludeFromCodeCoverage] public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Define as cores padrão no console.
        /// </summary>
        [ExcludeFromCodeCoverage] public void ResetColor() { }
        
        /// <summary>
        /// Escreve no console. 
        /// </summary>
        /// <param name="value">Texto.</param>
        public void Write(string value)
        {
            foreach (var ch in value)
            {
                if (ch >= 32)
                {
                    CursorLeft = CursorLeft < BufferWidth - 1 ? CursorLeft + 1 : 0;
                    if (CursorLeft == 0) CursorTop++;
                }
                else if (ch == '\n')
                {
                    CursorLeft = 0;
                    CursorTop++;
                }
            }

            if (CursorTop > BufferHeight) CursorTop = BufferHeight;  
        }

        /// <summary>
        /// Escreve no console com quebra de linha. 
        /// </summary>
        /// <param name="value">Texto.</param>
        public void WriteLine(string value) => Write(value + Environment.NewLine);

        /// <summary>
        /// Lê uma entrada de dados do usuário.
        /// </summary>
        /// <returns>Valor entrado.</returns>
        public string ReadLine()
        {
            var resposta = _resposta;
            _resposta = string.Empty;
            CursorLeft = 0;
            CursorTop += 1 + resposta.Length / BufferWidth;
            CursorTop = CursorTop <= BufferHeight ? CursorTop : BufferHeight;
            return resposta;
        }

        /// <summary>
        /// Lê uma entrada de tecla de dados do usuário.
        /// </summary>
        /// <returns>Caracter digitado.</returns>
        public ConsoleKeyInfo ReadKey()
        {
            if (_resposta.Length == 0) return new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);
            
            var resposta = _resposta[0];
            _resposta = _resposta.Substring(1);
            
            if (resposta == '\b') return new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false);

            Enum.TryParse(resposta.ToString().ToUpper(), out ConsoleKey consoleKey);
            return new ConsoleKeyInfo(resposta, consoleKey, false, false, false);
        }

        /// <summary>
        /// Verifica se existe tecla disponível para leitura imediata.
        /// </summary>
        [ExcludeFromCodeCoverage] public bool KeyAvailable { get; } = false;
        
        /// <summary>
        /// Posição do cursor: topo
        /// </summary>
        public int CursorTop { get; set; }
        
        /// <summary>
        /// Posição do cursor: esquerda
        /// </summary>
        public int CursorLeft { get; set; }

        /// <summary>
        /// Comprimento do cursor: Altura
        /// </summary>
        public int BufferHeight { get; } = 25;

        /// <summary>
        /// Comprimento do cursor: largura
        /// </summary>
        public int BufferWidth { get; } = 80;

        /// <summary>
        /// Define a posição do cursor.
        /// </summary>
        /// <param name="left">Posição do cursor: esquerda</param>
        /// <param name="top">Posição do cursor: topo</param>
        public void SetCursorPosition(int left, int top)
        {
            CursorLeft = left;
            CursorTop = top;
        }
    }
}