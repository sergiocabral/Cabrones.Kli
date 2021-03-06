﻿using System;
using Cabrones.Test;
using FluentAssertions;
using Kli.Wrappers;
using NSubstitute;
using Xunit;

namespace Kli.Output.Console
{
    public class TestOutputConsole
    {
        public TestOutputConsole()
        {
            Program.DependencyResolver.Register<IOutputConsole, OutputConsole>();
            Program.DependencyResolver.Register<IOutputMarkersToConsoleColor, OutputMarkersToConsoleColor>();
        }

        [Fact]
        public void ao_escrever_um_texto_o_método_WriteToConsole_deve_ser_chamado()
        {
            // Arrange, Given

            var outputWriter = Substitute.For<IOutputWriter>();
            var outputConsole = new OutputConsole(
                outputWriter,
                Substitute.For<IOutputMarkersToConsoleColor>(),
                Substitute.For<IConsole>()) as IOutputConsole;
            var textoDeExemplo = this.Fixture<string>();

            // Act, When

            outputConsole.Write(textoDeExemplo);
            outputConsole.WriteLine(textoDeExemplo);

            // Assert, Then

            outputWriter.Received(1).Parse(textoDeExemplo, outputConsole.WriteToConsole);
            outputWriter.Received(1).Parse($"{textoDeExemplo}\n", outputConsole.WriteToConsole);
        }

        [Fact]
        public void o_método_WriteToConsole_deve_escrever_no_console_em_cores_correspondentes_aos_marcadores()
        {
            // Arrange, Given

            var outputMarkers = Program.DependencyResolver.GetInstance<IOutputMarkers>();
            var outputMarkersToConsoleColor = Program.DependencyResolver.GetInstance<IOutputMarkersToConsoleColor>();
            var console = Substitute.For<IConsole>();
            var outputConsole = new OutputConsole(
                Program.DependencyResolver.GetInstance<IOutputWriter>(),
                outputMarkersToConsoleColor,
                console) as IOutputConsole;

            foreach (var marcador in outputMarkers.Markers)
            {
                console.ClearReceivedCalls();

                // Act, When

                var (esperadoParaForeground, esperadoParaBackground) = outputMarkersToConsoleColor.Convert(marcador);
                outputConsole.WriteToConsole(this.Fixture<string>(), marcador);

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
                Program.DependencyResolver.GetInstance<IOutputWriter>(),
                outputMarkersToConsoleColor,
                console) as IOutputConsole;

            outputMarkersToConsoleColor.Convert((char) 0).Returns(
                this.Fixture<Tuple<ConsoleColor, ConsoleColor>>());

            var textoDeExemplo = this.Fixture<string>();

            // Act, When

            outputConsole.WriteToConsole(textoDeExemplo);

            // Assert, Then

            console.Received(1).Write(textoDeExemplo);
        }

        [Fact]
        public void
            o_método_WriteToConsole_deve_usar_IOutputMarkersToConsoleColor_para_formatar_de_acordo_com_o_marcador()
        {
            // Arrange, Given

            var outputMarkersToConsoleColor = Substitute.For<IOutputMarkersToConsoleColor>();
            var outputConsole = new OutputConsole(
                Program.DependencyResolver.GetInstance<IOutputWriter>(),
                outputMarkersToConsoleColor,
                Substitute.For<IConsole>()) as IOutputConsole;

            var marcadorDeExemplo = this.Fixture<char>();

            outputMarkersToConsoleColor.Convert(marcadorDeExemplo).Returns(
                this.Fixture<Tuple<ConsoleColor, ConsoleColor>>());

            // Act, When

            outputConsole.WriteToConsole(null, marcadorDeExemplo);

            // Assert, Then

            outputMarkersToConsoleColor.Received(1).Convert(marcadorDeExemplo);
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

        [Fact]
        public void verifica_se_o_marcador_atual_foi_determinado_corretamente_usando_IConsole()
        {
            // Arrange, Given

            var outputMarkersToConsoleColor = Program.DependencyResolver.GetInstance<IOutputMarkersToConsoleColor>();
            var console = Substitute.For<IConsole>();
            var outputConsole = new OutputConsole(
                Substitute.For<IOutputWriter>(),
                outputMarkersToConsoleColor,
                console) as IOutputConsole;

            var marcadorAtualEsperado =
                outputMarkersToConsoleColor.Convert(console.ForegroundColor, console.BackgroundColor);

            var consultaDeBackgroundColor = 0;
            console.BackgroundColor.Returns(info =>
            {
                consultaDeBackgroundColor++;
                return default;
            });

            var consultaDeForegroundColor = 0;
            console.ForegroundColor.Returns(info =>
            {
                consultaDeForegroundColor++;
                return default;
            });

            // Act, When

            var marcadorAtual = outputConsole.CurrentMarker();

            // Assert, Then

            consultaDeBackgroundColor.Should().Be(1);
            consultaDeForegroundColor.Should().Be(1);
            marcadorAtual.Should().Be(marcadorAtualEsperado);
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(OutputConsole);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IOutput),
                typeof(IOutputConsole));
            sut.AssertMyOwnImplementations(
                typeof(IOutputConsole));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}