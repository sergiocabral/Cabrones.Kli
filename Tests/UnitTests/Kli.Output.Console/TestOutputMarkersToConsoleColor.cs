using System;
using FluentAssertions;
using Kli.IO;
using Kli.Output.Console;
using Xunit;

namespace Tests.UnitTests.Kli.Output.Console
{
    public class TestOutputMarkersToConsoleColor: Test
    {
        public TestOutputMarkersToConsoleColor()
        {
            DependencyResolverFromProgram.Register<IOutputMarkersToConsoleColor, OutputMarkersToConsoleColor>();    
        }
        
        [Theory]
        [InlineData(typeof(OutputMarkersToConsoleColor), typeof(IOutputMarkersToConsoleColor))]
        public void verifica_se_classe_implementa_tipos(Type tipoDaClasse, Type tipoQueDeveSerImplementado)
        {
            verifica_se_classe_implementa_tipo(tipoDaClasse, tipoQueDeveSerImplementado);
        }

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

            var outputMarkersToConsoleColor = DependencyResolverFromProgram.GetInstance<IOutputMarkersToConsoleColor>();

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

            var outputMarkersToConsoleColor = DependencyResolverFromProgram.GetInstance<IOutputMarkersToConsoleColor>();

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