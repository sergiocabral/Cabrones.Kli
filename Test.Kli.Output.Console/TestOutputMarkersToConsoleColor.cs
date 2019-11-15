using System;
using FluentAssertions;
using Cabrones.Test;
using Xunit;

namespace Kli.Output.Console
{
    public class TestOutputMarkersToConsoleColor
    {
        public TestOutputMarkersToConsoleColor()
        {
            Program.DependencyResolver.Register<IOutputMarkersToConsoleColor, OutputMarkersToConsoleColor>();    
        }
        
        [Theory]
        [InlineData(typeof(OutputMarkersToConsoleColor), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestTypeMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(OutputMarkersToConsoleColor), typeof(IOutputMarkersToConsoleColor))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            tipoDaClasse.TestTypeImplementations(tiposQueDeveSerImplementado);

        [Theory]
        [InlineData((char) 0, ConsoleColor.Gray, ConsoleColor.Black)]
        [InlineData('#', ConsoleColor.DarkGray, ConsoleColor.Black)]
        [InlineData('_', ConsoleColor.Magenta, ConsoleColor.Black)]
        [InlineData('*', ConsoleColor.DarkCyan, ConsoleColor.Black)]
        [InlineData('?', ConsoleColor.DarkYellow, ConsoleColor.Black)]
        [InlineData('@', ConsoleColor.Yellow, ConsoleColor.Black)]
        [InlineData('!', ConsoleColor.Red, ConsoleColor.Black)]
        public void verifica_se_as_cores_estão_corretamente_associadas(char marcador, ConsoleColor foreground, ConsoleColor background)
        {
            // Arrange, Given

            var outputMarkersToConsoleColor = Program.DependencyResolver.GetInstance<IOutputMarkersToConsoleColor>();

            // Act, When

            var convertidoParaMarcador = outputMarkersToConsoleColor.Convert(foreground, background);
            var (convertidoParaForeground, convertidoParaBackground) = outputMarkersToConsoleColor.Convert(marcador);
            
            // Assert, Then

            convertidoParaMarcador.Should().Be(marcador);
            convertidoParaForeground.Should().Be(foreground);
            convertidoParaBackground.Should().Be(background);
        }

        [Fact]
        public void valores_desconhecidos_devem_retornar_o_valor_padrão()
        {
            // Arrange, Given

            var outputMarkersToConsoleColor = Program.DependencyResolver.GetInstance<IOutputMarkersToConsoleColor>();

            // Act, When

            var convertidoParaMarcador = outputMarkersToConsoleColor.Convert(ConsoleColor.White, ConsoleColor.White);
            var (convertidoParaForeground, convertidoParaBackground) = outputMarkersToConsoleColor.Convert('Z');
            
            // Assert, Then

            convertidoParaMarcador.Should().Be((char) 0);
            convertidoParaForeground.Should().Be(ConsoleColor.Gray);
            convertidoParaBackground.Should().Be(ConsoleColor.Black);
        }
    }
}