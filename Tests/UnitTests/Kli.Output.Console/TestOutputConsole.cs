using System;
using AutoFixture;
using Kli.IO;
using Kli.Output.Console;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.Output.Console
{
    public class TestOutputConsole: Test
    {
        public TestOutputConsole()
        {
            DependencyResolverFromProgram.Register<IOutputConsole, OutputConsole>();    
        }
        
        [Theory]
        [InlineData(typeof(OutputConsole), typeof(IOutput))]
        public void verifica_se_classe_implementa_tipos(Type tipoDaClasse, Type tipoQueDeveSerImplementado)
        {
            verifica_se_classe_implementa_tipo(tipoDaClasse, tipoQueDeveSerImplementado);
        }

        [Fact]
        public void ao_escrever_um_texto_o_método_WriteToConsole_deve_ser_chamado()
        {
            // Arrange, Given

            var outputWriter = Substitute.For<IOutputWriter>();
            var outputConsole = new OutputConsole(
                outputWriter,
                Substitute.For<IOutputMarkersToConsoleColor>());
            var textoDeExemplo = Fixture.Create<string>();
            
            // Act, When

            outputConsole.Write(textoDeExemplo);
            outputConsole.WriteLine(textoDeExemplo);
            
            // Assert, Then

            outputWriter.Received(1).Parse(textoDeExemplo, outputConsole.WriteToConsole);
            outputWriter.Received(1).Parse($"{textoDeExemplo}\n", outputConsole.WriteToConsole);
        }

        [Fact]
        public void o_método_WriteToConsole_deve_usar_IOutputMarkersToConsoleColor_para_formatar_de_acordo_com_o_marcador()
        {
            // Arrange, Given

            var outputMarkersToConsoleColor = Substitute.For<IOutputMarkersToConsoleColor>();
            var outputConsole = new OutputConsole(
                DependencyResolverFromProgram.GetInstance<IOutputWriter>(),
                outputMarkersToConsoleColor);
            
            var marcadorDeExemplo = Fixture.Create<char>();
            
            outputMarkersToConsoleColor.Convert(marcadorDeExemplo).Returns(
                info => new Tuple<ConsoleColor, ConsoleColor>(
                    Fixture.Create<ConsoleColor>(),
                    Fixture.Create<ConsoleColor>()));
            
            // Act, When

            outputConsole.WriteToConsole(null, marcadorDeExemplo);
            
            // Assert, Then

            outputMarkersToConsoleColor.Received(1).Convert(marcadorDeExemplo);
        }
    }
}