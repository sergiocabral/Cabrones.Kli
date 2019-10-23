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
            DependencyResolverFromProgram.Register<IOutputMarkersToConsoleColor, OutputMarkersToConsoleColor>();
        }
        
        [Theory]
        [InlineData(typeof(OutputConsole), typeof(IOutput))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, Type tipoQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tipoQueDeveSerImplementado);

        [Fact]
        public void ao_escrever_um_texto_o_método_WriteToConsole_deve_ser_chamado()
        {
            // Arrange, Given

            var outputWriter = Substitute.For<IOutputWriter>();
            var outputConsole = new OutputConsole(
                outputWriter,
                Substitute.For<IOutputMarkersToConsoleColor>(),
                Substitute.For<IConsole>());
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
                outputMarkersToConsoleColor,
                Substitute.For<IConsole>());
            
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

        [Fact]
        public void o_método_WriteToConsole_deve_escrever_no_console_em_cores_correspondentes_aos_marcadores()
        {
            // Arrange, Given

            var outputMarkers = DependencyResolverFromProgram.GetInstance<IOutputMarkers>();
            var outputMarkersToConsoleColor = DependencyResolverFromProgram.GetInstance<IOutputMarkersToConsoleColor>();
            var console = Substitute.For<IConsole>();
            var outputConsole = new OutputConsole(
                DependencyResolverFromProgram.GetInstance<IOutputWriter>(),
                outputMarkersToConsoleColor,
                console);

            foreach (var marcador in outputMarkers.Markers)
            {
                console.ClearReceivedCalls();
                    
                // Act, When

                var (esperadoParaForeground, esperadoParaBackground) = outputMarkersToConsoleColor.Convert(marcador);
                outputConsole.WriteToConsole(Fixture.Create<string>(), marcador);
            
                // Assert, Then

                console.Received(1).BackgroundColor = esperadoParaBackground;
                console.Received(1).ForegroundColor = esperadoParaForeground;
            }
        }

        [Fact]
        public void o_método_WriteToConsole_deve_usar_IConsole_para_escrever_de_fato_no_console()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var outputMarkersToConsoleColor = Substitute.For<IOutputMarkersToConsoleColor>();
            var outputConsole = new OutputConsole(
                DependencyResolverFromProgram.GetInstance<IOutputWriter>(),
                outputMarkersToConsoleColor,
                console);
            
            outputMarkersToConsoleColor.Convert((char)0).Returns(
                info => new Tuple<ConsoleColor, ConsoleColor>(
                    Fixture.Create<ConsoleColor>(),
                    Fixture.Create<ConsoleColor>()));
            
            var textoDeExemplo = Fixture.Create<string>();
            
            // Act, When

            outputConsole.WriteToConsole(textoDeExemplo);
            
            // Assert, Then

            console.Received(1).Write(textoDeExemplo);
        }
    }
}