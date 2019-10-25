using System;
using AutoFixture;
using FluentAssertions;
using Kli.IO;
using Kli.Output.Console;
using Kli.Wrappers;
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
        [InlineData(typeof(OutputConsole), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(OutputConsole), typeof(IOutput), typeof(IOutputConsole))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

        [Fact]
        public void ao_escrever_um_texto_o_método_WriteToConsole_deve_ser_chamado()
        {
            // Arrange, Given

            var outputWriter = Substitute.For<IOutputWriter>();
            var outputConsole = new OutputConsole(
                outputWriter,
                Substitute.For<IOutputMarkersToConsoleColor>(),
                Substitute.For<IConsole>()) as IOutputConsole;
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
                Substitute.For<IConsole>()) as IOutputConsole;
            
            var marcadorDeExemplo = Fixture.Create<char>();
            
            outputMarkersToConsoleColor.Convert(marcadorDeExemplo).Returns(
                info => Fixture.Create<Tuple<ConsoleColor, ConsoleColor>>());
            
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
                console) as IOutputConsole;

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
                console) as IOutputConsole;
            
            outputMarkersToConsoleColor.Convert((char)0).Returns(
                info => Fixture.Create<Tuple<ConsoleColor, ConsoleColor>>());
            
            var textoDeExemplo = Fixture.Create<string>();
            
            // Act, When

            outputConsole.WriteToConsole(textoDeExemplo);
            
            // Assert, Then

            console.Received(1).Write(textoDeExemplo);
        }
        
        [Fact]
        public void os_métodos_de_escrita_devem_retornar_uma_auto_referência()
        {
            // Arrange, Given

            var outputConsole = new OutputConsole(
                Substitute.For<IOutputWriter>(),
                Substitute.For<IOutputMarkersToConsoleColor>(),
                Substitute.For<IConsole>()) as IOutputConsole;
            
            // Act, When

            var retornoDeWrite = outputConsole.Write(null);
            var retornoDeWriteLine = outputConsole.WriteLine(null);
            
            // Assert, Then

            retornoDeWrite.Should().BeSameAs(outputConsole);
            retornoDeWriteLine.Should().BeSameAs(outputConsole);
        }
    }
}