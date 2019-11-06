using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Kli.Wrappers;
using Xunit;
using Environment = System.Environment;

namespace Kli.Input.Console
{
    /// <summary>
    /// Classe de teste para garantir que o teste de cobertura chega a 100%.
    /// </summary>
    public class TestSimulationForIConsole
    {
        [Fact]
        public void fazer_teste_de_cobertura_chegar_a_100_porcento()
        {
            // Arrange, Given

            var console = new SimulationForIConsole(string.Empty) as IConsole;
            
            // Act, When

            Action definirValorForaDoRangeParaCursorTop = () => console.CursorTop = -1; 
            Action definirValorForaDoRangeParaCursorLeft = () => console.CursorLeft = -1; 

            // Assert, Then

            definirValorForaDoRangeParaCursorTop.Should().ThrowExactly<ArgumentOutOfRangeException>();
            definirValorForaDoRangeParaCursorLeft.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }
    
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
            BufferHeight = 25;
            BufferWidth = 80;
            CursorTop = 0;
            CursorLeft = 0;
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
        /// Texto escrito no console.
        /// </summary>
        public string OQueFoiEscrito { get; private set; } = string.Empty;
        
        /// <summary>
        /// Escreve no console. 
        /// </summary>
        /// <param name="value">Texto.</param>
        public void Write(string value)
        {
            var cursorLeft = CursorLeft;
            var cursorTop = CursorTop;
            
            foreach (var ch in value)
            {
                if (ch >= 32)
                {
                    cursorLeft = cursorLeft < BufferWidth - 1 ? cursorLeft + 1 : 0;
                    if (cursorLeft == 0) cursorTop++;
                }
                else if (ch == '\n')
                {
                    cursorLeft = 0;
                    cursorTop++;
                }
            }

            if (cursorTop >= BufferHeight) cursorTop = BufferHeight - 1;
            
            CursorLeft = cursorLeft;
            CursorTop = cursorTop;
            
            OQueFoiEscrito += value;
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
            OQueFoiEscrito += resposta + Environment.NewLine;
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

            var valueForConsoleKey =
                Regex.IsMatch(resposta.ToString(), "[0-9]") ? 
                    "NumPad" + resposta : 
                    resposta.ToString().ToUpper();

            Enum.TryParse(valueForConsoleKey, out ConsoleKey consoleKey);
            return new ConsoleKeyInfo(resposta, consoleKey, false, false, false);
        }

        /// <summary>
        /// Verifica se existe tecla disponível para leitura imediata.
        /// </summary>
        [ExcludeFromCodeCoverage] public bool KeyAvailable { get; } = false;
        
        /// <summary>
        /// Histórico de valores.
        /// </summary>
        public Dictionary<string, IList<int>> Historico { get; } = new Dictionary<string, IList<int>>();

        /// <summary>
        /// Posição do cursor: topo
        /// </summary>
        public int CursorTop
        {
            get => Historico[nameof(CursorTop)].Last();
            set
            {
                if (value < 0 || value >= BufferHeight) throw new ArgumentOutOfRangeException();
                const string key = nameof(CursorTop);
                if (!Historico.ContainsKey(key)) Historico[key] = new List<int>();
                Historico[key].Add(value);
            }  
        }

        /// <summary>
        /// Posição do cursor: esquerda
        /// </summary>
        public int CursorLeft
        {
            get => Historico[nameof(CursorLeft)].Last();
            set
            {
                if (value < 0 || value >= BufferWidth) throw new ArgumentOutOfRangeException();
                const string key = nameof(CursorLeft);
                if (!Historico.ContainsKey(key)) Historico[key] = new List<int>();
                Historico[key].Add(value);
            }  
        }

        /// <summary>
        /// Comprimento do cursor: Altura
        /// </summary>
        public int BufferHeight
        {
            get => Historico[nameof(BufferHeight)].Last();
            set
            {
                const string key = nameof(BufferHeight);
                if (!Historico.ContainsKey(key)) Historico[key] = new List<int>();
                Historico[key].Add(value);
            }  
        }

        /// <summary>
        /// Comprimento do cursor: largura
        /// </summary>
        public int BufferWidth
        {
            get => Historico[nameof(BufferWidth)].Last();
            set
            {
                const string key = nameof(BufferWidth);
                if (!Historico.ContainsKey(key)) Historico[key] = new List<int>();
                Historico[key].Add(value);
            }  
        }

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