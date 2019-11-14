using System;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Kli.Wrappers;
using NSubstitute;
using Test;
using Xunit;
using Environment = System.Environment;

namespace Kli.Input.Console
{
    public class TestInputConsole: BaseForTest
    {
        public TestInputConsole()
        {
            DependencyResolverFromProgram.Register<IInputConsole, InputConsole>();
        }
        
        [Theory]
        [InlineData(typeof(InputConsole), 4)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(InputConsole), typeof(IInput), typeof(IInputConsole))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            TestTypeImplementations(tipoDaClasse, tiposQueDeveSerImplementado);
        
        [Fact]
        public void os_métodos_Read_e_ReadLine_devem_chamar_o_método_correspondente_de_Console()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var inputConsole = new InputConsole(console);
            
            // Act, When

            inputConsole.Read();
            inputConsole.ReadLine();

            // Assert, Then

            console.Received(2).ReadLine();
        }
        
        [Fact]
        public void o_método_ReadKey_deve_chamar_o_método_correspondente_de_Console()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var inputConsole = new InputConsole(console);
            
            // Act, When

            inputConsole.ReadKey();

            // Assert, Then

            console.Received(1).ReadKey();
        }
        
        [Fact]
        public void o_método_HasRead_deve_chamar_o_método_correspondente_de_Console()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var inputConsole = new InputConsole(console);

            var quantasVezesFoiChamado = 0;
            console.KeyAvailable.Returns(quantasVezesFoiChamado++ >= 0);
            
            // Act, When

            inputConsole.HasRead();

            // Assert, Then

            quantasVezesFoiChamado.Should().Be(1);
        }
        
        [Fact]
        public void os_métodos_Read_e_ReadLine_não_devem_chamar_Console_ReadLine_quando_isSensitive_é_true()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var inputConsole = new InputConsole(console);

            console.ReadKey().Returns(
                new ConsoleKeyInfo((char) 13, ConsoleKey.Enter, false, false, false));
            
            // Act, When

            inputConsole.Read(true);
            inputConsole.ReadLine(true);

            // Assert, Then
            
            console.Received(0).ReadLine();
        }
        
        [Fact]
        public void os_métodos_Read_e_ReadLine_deve_exibir_asterisco_para_cada_letra_digitada_quando_isSensitive_é_true()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var inputConsole = new InputConsole(console);

            const string resposta = "senha";
            var index = 0;
            console.ReadKey().Returns(info =>
            {
                var consoleKey = ConsoleKey.Enter;
                const char ch = (char) 13;

                if (index++ < resposta.Length &&
                    Enum.TryParse(resposta[index - 1].ToString().ToUpper(), out consoleKey))
                    return new ConsoleKeyInfo(resposta[index - 1], consoleKey, false, false, false);
                
                index = 0;
                return new ConsoleKeyInfo(ch, consoleKey, false, false, false);
            });
            
            // Act, When

            inputConsole.Read(true);
            inputConsole.ReadLine(true);

            // Assert, Then
            
            console.Received(resposta.Length * 2).Write("*");
        }
        
        [Fact]
        public void o_método_Read_deve_apagar_o_texto_digitado_e_retornar_o_cursor_para_a_posição_inicial()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var inputConsole = new InputConsole(console);

            const string resposta = "texto";
            console.BufferWidth.Returns(80);
            console.ReadLine().Returns(resposta);

            var cursorLeft = console.CursorLeft;
            var cursorTop = console.CursorTop;
            
            // Act, When

            inputConsole.Read();

            // Assert, Then
            
            console.Received(1).WriteLine(new string(' ', resposta.Length));
            console.Received(2).SetCursorPosition(cursorLeft, cursorTop > 0 ? cursorTop - 1 : 0); // Por causa do avanço de linha deve ser CursorTop-1
        }
        
        [Fact]
        public void o_método_ReadLine_deve_fazer_o_processamento_da_tecla_backspace_quando_isSensitive_é_true()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var inputConsole = new InputConsole(console);

            const string resposta = "senha123\b\b\b";
            var index = 0;
            console.ReadKey().Returns(info =>
            {
                var consoleKey = ConsoleKey.Enter;
                var ch = (char) 13;

                if (index++ < resposta.Length &&
                    Enum.TryParse(resposta[index - 1].ToString().ToUpper(), out consoleKey))
                    return new ConsoleKeyInfo(resposta[index - 1], consoleKey, false, false, false);

                if (index <= resposta.Length && resposta[index - 1] == '\b')
                {
                    consoleKey = ConsoleKey.Backspace;
                    ch = resposta[index - 1];
                }
                else
                {
                    index = 0;
                }

                return new ConsoleKeyInfo(ch, consoleKey, false, false, false);
            });
            
            // Act, When

            inputConsole.ReadLine(true);

            // Assert, Then
            
            console.Received(8).Write("*");
            console.Received(3).Write(" ");
        }

        [Theory]
        [InlineData(0, 0, "texto pequeno")]
        [InlineData(10, 0, "texto pequeno")]
        [InlineData(0, 10, "texto pequeno")]
        [InlineData(10, 10, "texto pequeno")]
        [InlineData(0, 0, "texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior.")]
        [InlineData(10, 0, "texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior.")]
        [InlineData(0, 10, "texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior.")]
        [InlineData(10, 10, "texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior.")]
        [InlineData(24, 10, "texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior, texto bem maior.")]
        public void verifica_se_ReadLine_com_isSensitive_está_movimentando_o_cursor_corretamente(int posiçãoInicialCursorTop, int posiçãoInicialCursorLeft, string texto)
        {
            // Arrange, Given

            var console = new SimulationForIConsole(texto) as IConsole;
            var inputConsole = new InputConsole(console);
            console.CursorTop = posiçãoInicialCursorTop;
            console.CursorLeft = posiçãoInicialCursorLeft;
            
            // Act, When

            inputConsole.ReadLine(true);

            // Assert, Then

            var esperadoParaCursorTop = posiçãoInicialCursorTop + 1 + texto.Length / console.BufferWidth;
            if (esperadoParaCursorTop > console.BufferHeight) esperadoParaCursorTop = console.BufferHeight - 1;

            console.CursorTop.Should().Be(esperadoParaCursorTop);
            console.CursorLeft.Should().Be(0);
        }
        
        [Theory]
        [InlineData(true, 0, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(true, 10, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(true, 0, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(true, 10, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(true, 0, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        [InlineData(true, 10, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        [InlineData(true, 0, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        [InlineData(true, 10, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        [InlineData(false, 0, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(false, 10, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(false, 0, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(false, 10, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente")]
        [InlineData(false, 0, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        [InlineData(false, 10, 0, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        [InlineData(false, 0, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        [InlineData(false, 10, 10, "teste\b\b\b\b\b\b\b\b\b\b\b\b\b\bnovamente muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto-muito-texto.")]
        public void verifica_se_Read_está_posicionamento_o_cursor_na_posição_inicial_e_apagando_o_texto_digitado(bool isSensitive, int posiçãoInicialCursorTop, int posiçãoInicialCursorLeft, string texto)
        {
            // Arrange, Given

            var console = new SimulationForIConsole(texto);
            var inputConsole = new InputConsole(console);
            var cursorTop  = console.CursorTop = posiçãoInicialCursorTop;
            var cursorLeft = console.CursorLeft = posiçãoInicialCursorLeft;
            
            // Act, When

            inputConsole.Read(isSensitive);

            // Assert, Then

            console.CursorTop.Should().Be(cursorTop);
            console.CursorLeft.Should().Be(cursorLeft);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ao_final_de_ReadLine_deve_escrever_o_texto_e_avançar_uma_linha(bool isSensitive)
        {
            // Arrange, Given

            var texto = Fixture.Create<string>();
            var console = new SimulationForIConsole(texto);
            var inputConsole = new InputConsole(console);
            var textoEsperado = !isSensitive ? texto : new string('*', texto.Length); 
            
            // Act, When

            inputConsole.ReadLine(isSensitive);

            // Assert, Then

            console.OQueFoiEscrito.Should().Be(textoEsperado + Environment.NewLine);
        }

        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("abc\b", 0, 2)]
        [InlineData("abc\b\b\b", 0, 0)]
        [InlineData("abc\b\b\b\b\b\b", 0, 0)]
        public void confere_posicionamento_do_cursor_ao_usar_backspace_com_ReadLine_isSensitive(string texto, int posiçãoEsperadaAntesDoEnterDeCursorTop, int posiçãoEsperadaAntesDoEnterDeCursorLeft)
        {
            // Arrange, Given
            
            var console = new SimulationForIConsole(texto);
            var inputConsole = new InputConsole(console);

            const int posiçãoInicialDeCursorTop = 10;
            console.CursorTop = posiçãoInicialDeCursorTop;

            string TextoComBackspaceProcessado()
            {
                var resultado = new StringBuilder();
                
                foreach (var ch in texto)
                {
                    if (ch != '\b') resultado.Append(ch);
                    else if (resultado.Length > 0) resultado.Remove(resultado.Length - 1, 1);
                }

                return resultado.ToString();
            }

            var textoComBackspaceProcessado = TextoComBackspaceProcessado();
            
            // Act, When

            inputConsole.ReadLine(true);

            // Assert, Then

            var historicoCursorTop = console.Historico["CursorTop"];
            var historicoCursorLeft = console.Historico["CursorLeft"];
            
            historicoCursorTop[^2].Should().Be(posiçãoInicialDeCursorTop + posiçãoEsperadaAntesDoEnterDeCursorTop);
            historicoCursorLeft[^2].Should().Be(posiçãoEsperadaAntesDoEnterDeCursorLeft);
            
            if (textoComBackspaceProcessado.Length <= 0) return;
            
            historicoCursorLeft[^2].Should().Be(textoComBackspaceProcessado.Length);
        }

        [Theory]
        [InlineData("ab", 0, 2)]
        [InlineData("ab\b", 0, 1)]
        [InlineData("ab\b\b\b", 0, 0)]
        [InlineData("abc", 1, 0)]
        [InlineData("abcABC", 2, 0)]
        [InlineData("abcABCasa", 2, 0)]
        [InlineData("abcABCasa\b", 1, 2)]
        [InlineData("abcABCasa\b\b\b\b\b\b\b\b\b", 0, 0)]
        [InlineData("abcABCasa\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b", 0, 0)]
        public void num_console_de_3x3_confere_posicionamento_do_cursor_entre_linhas_ao_usar_backspace_com_ReadLine_isSensitive(string texto, int posiçãoEsperadaAntesDoEnterDeCursorTop, int posiçãoEsperadaAntesDoEnterDeCursorLeft)
        {
            // Arrange, Given
            
            var console = new SimulationForIConsole(texto) { BufferWidth = 3, BufferHeight = 3 };
            var inputConsole = new InputConsole(console);
            
            // Act, When

            inputConsole.ReadLine(true);

            // Assert, Then

            var historicoCursorTop = console.Historico["CursorTop"];
            var historicoCursorLeft = console.Historico["CursorLeft"];
            
            historicoCursorTop[^2].Should().Be(posiçãoEsperadaAntesDoEnterDeCursorTop);
            historicoCursorLeft[^2].Should().Be(posiçãoEsperadaAntesDoEnterDeCursorLeft);
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("ab\b")]
        [InlineData("ab\b\b\b")]
        [InlineData("abc")]
        [InlineData("abcABC")]
        [InlineData("abcABCasa")]
        [InlineData("abcABCasa\b")]
        [InlineData("abcABCasa\b\b\b\b\b\b\b\b\b")]
        [InlineData("abcABCasa\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b")]
        public void num_console_de_3x3_confere_posicionamento_do_cursor_entre_linhas_ao_usar_backspace_com_Read_isSensitive(string texto)
        {
            // Arrange, Given
            
            var console = new SimulationForIConsole(texto) { BufferWidth = 3, BufferHeight = 3 };
            var inputConsole = new InputConsole(console);
            
            // Act, When

            inputConsole.Read(true);

            // Assert, Then

            var historicoCursorTop = console.Historico["CursorTop"];
            var historicoCursorLeft = console.Historico["CursorLeft"];
            
            historicoCursorTop[^1].Should().Be(0);
            historicoCursorLeft[^1].Should().Be(0);
        }

        [Theory]
        [InlineData("", 1, 1)]
        [InlineData("b", 1, 1)]
        [InlineData("bc", 0, 1)]
        [InlineData("bc123456789", 0, 1)]
        [InlineData("bc123456789\b\b\b\b\b\b\b\b\b\b\b", 0, 1)]
        [InlineData("bc123456789\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b", 0, 1)]
        public void num_console_de_3x3_confere_posicionamento_do_cursor_entre_linhas_quando_posicionado_em_1x1_ao_usar_backspace_com_Read_isSensitive(string texto, int posiçãoEsperadaAntesDoEnterDeCursorTop, int posiçãoEsperadaAntesDoEnterDeCursorLeft)
        {
            // Arrange, Given
            
            var console = new SimulationForIConsole(texto)
            {
                BufferWidth = 3, BufferHeight = 3, 
                CursorLeft = 1, CursorTop = 1
            };
            var inputConsole = new InputConsole(console);
            
            // Act, When

            inputConsole.Read(true);

            // Assert, Then

            var historicoCursorTop = console.Historico["CursorTop"];
            var historicoCursorLeft = console.Historico["CursorLeft"];
            
            historicoCursorTop[^1].Should().Be(posiçãoEsperadaAntesDoEnterDeCursorTop);
            historicoCursorLeft[^1].Should().Be(posiçãoEsperadaAntesDoEnterDeCursorLeft);
        }
    }
}