using System;
using Kli.Wrappers;

namespace Kli.Input.Console
{
    /// <summary>
    /// Envia dados para o usuário via console.
    /// </summary>
    public class InputConsole: IInputConsole
    {
        /// <summary>
        /// Define as cores padrão no console.
        /// </summary>
        private readonly IConsole _console;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="console">Define as cores padrão no console.</param>
        public InputConsole(IConsole console)
        {
            _console = console;
        }

        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas espera concluir com Enter.
        /// Avança linha após o Enter.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        public string ReadLine(bool isSensitive = false)
        {
            if (!isSensitive) return _console.ReadLine();

            const string sensitiveChar = "*";
            
            var answer = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = _console.ReadKey();
                if (key.Key == ConsoleKey.Backspace && answer.Length > 0)
                {
                    //Processa o backspace.
                    answer = answer.Substring(0, answer.Length - 1);
                    
                    _console.CursorTop = _console.CursorLeft > 0 ? _console.CursorTop : _console.CursorTop > 0 ? _console.CursorTop - 1 : _console.CursorTop;
                    _console.CursorLeft = _console.CursorLeft > 0 ? _console.CursorLeft - 1 : _console.BufferWidth - 1;
                    
                    _console.Write(" ");
                    
                    _console.CursorTop = _console.CursorLeft > 0 ? _console.CursorTop : _console.CursorTop > 0 ? _console.CursorTop - 1 : _console.CursorTop;
                    _console.CursorLeft = _console.CursorLeft > 0 ? _console.CursorLeft - 1 : _console.BufferWidth - 1;
                } else if (key.KeyChar >= 32)
                {
                    //Adiciona apenas caracteres válidos.
                    answer += key.KeyChar;
                    _console.Write(sensitiveChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            _console.WriteLine(string.Empty);

            return answer;
        }

        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas espera concluir com Enter.
        /// Não avança linha após o Enter e apaga o texto digitado.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        public string Read(bool isSensitive = false)
        {
            var cursorLeft = _console.CursorLeft;
            var answer = ReadLine(isSensitive) ?? string.Empty;
            var cursorTop = _console.CursorTop - 1;

            var lengthExtra = answer.Length - (_console.BufferWidth - cursorLeft);

            if (lengthExtra > 0) cursorTop -= (int) Math.Floor((double) lengthExtra / _console.BufferWidth) + 1;

            _console.SetCursorPosition(cursorLeft, cursorTop);
            _console.WriteLine(new string(' ', answer.Length));
            _console.SetCursorPosition(cursorLeft, cursorTop);

            return answer;
        }

        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas conclui imediatamente na primeira tecla.
        /// </summary>
        /// <returns>Caracter recebido.</returns>
        public string ReadKey() => _console.ReadKey().KeyChar.ToString();

        /// <summary>
        /// Verifica se possui resposta disponível.
        /// </summary>
        public bool HasRead() => _console.KeyAvailable;
    }
}