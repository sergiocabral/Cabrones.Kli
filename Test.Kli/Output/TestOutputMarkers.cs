﻿using System;
using System.Linq;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Output
{
    public class TestOutputMarkers
    {
        [Theory]
        [InlineData(typeof(IOutputMarkers), "Markers")]
        [InlineData(typeof(IOutputMarkers), "MarkersEscapedForRegexJoined")]
        [InlineData(typeof(IOutputMarkers), "MarkersEscapedForRegexSeparated")]
        public void verifica_se_o_cache_está_sendo_usado_nas_consultas(Type tipo, string nomeDePropriedade)
        {
            Program.DependencyResolver.GetInstance(tipo).AssertTheSameValueButTheSecondTimeIsFaster(nomeDePropriedade);
        }

        [Fact]
        public void lista_de_marcadores_como_array_e_como_texto_devem_ser_iguais()
        {
            // Arrange, Given

            var outputFormatter = Program.DependencyResolver.GetInstance<IOutputMarkers>();

            // Act, When

            var comoArray = outputFormatter.MarkersEscapedForRegexSeparated;
            var comoTexto = outputFormatter.MarkersEscapedForRegexJoined;

            // Assert, Then

            comoTexto.Should().Be(string.Join("", comoArray));
        }

        [Fact]
        public void verifica_se_lista_de_marcadores_contem_todos_os_valores()
        {
            // Arrange, Given

            var outputFormatter = Program.DependencyResolver.GetInstance<IOutputMarkers>();

            // Act, When

            var markers = outputFormatter.Markers.ToList();

            // Assert, Then

            markers.Should().Contain(outputFormatter.Error);
            markers.Should().Contain(outputFormatter.Question);
            markers.Should().Contain(outputFormatter.Answer);
            markers.Should().Contain(outputFormatter.Highlight);
            markers.Should().Contain(outputFormatter.Light);
            markers.Should().Contain(outputFormatter.Low);
            markers.Should().HaveCount(6);
        }

        [Fact]
        public void verifica_se_os_caracteres_tipo_marcadores_podem_ser_escapados()
        {
            // Arrange, Given

            var outputFormatter = Program.DependencyResolver.GetInstance<IOutputMarkers>();

            foreach (var marcador in outputFormatter.Markers)
            {
                var texto = $"marcador: {marcador}.";

                // Act, When

                var textoEscapado = outputFormatter.Escape(texto);

                // Assert, Then

                textoEscapado.Should().Be($"marcador: {marcador}{marcador}.");
            }
        }

        [Fact]
        public void verifica_se_os_valores_dos_marcadores_estao_corretos()
        {
            // Arrange, Given

            var outputFormatter = Program.DependencyResolver.GetInstance<IOutputMarkers>();

            // Act, When

            var markerError = outputFormatter.Error;
            var markerQuestion = outputFormatter.Question;
            var markerAnswer = outputFormatter.Answer;
            var markerHighlight = outputFormatter.Highlight;
            var markerLight = outputFormatter.Light;
            var markerLow = outputFormatter.Low;

            // Assert, Then

            markerError.Should().Be('!');
            markerQuestion.Should().Be('?');
            markerAnswer.Should().Be('@');
            markerHighlight.Should().Be('*');
            markerLight.Should().Be('_');
            markerLow.Should().Be('#');
        }

        [Fact]
        public void verifica_se_texto_vazio_ou_em_branco_ou_sem_marcadores_é_prontamente_retornada_sem_fazer_análise()
        {
            // Arrange, Given

            var outputFormatter = Program.DependencyResolver.GetInstance<IOutputMarkers>();

            // Act, When

            var tempoParaTextoComMarcador =
                new Func<string>(() => outputFormatter.Escape(outputFormatter.MarkersEscapedForRegexJoined))
                    .StopwatchFor().Item1;
            var tempoParaTextoSemMarcador =
                new Func<string>(() => outputFormatter.Escape("12345")).StopwatchFor().Item1;
            var tempoParaTextoVazio = new Func<string>(() => outputFormatter.Escape(string.Empty)).StopwatchFor().Item1;
            var tempoParaTextoEmBranco = new Func<string>(() => outputFormatter.Escape("     ")).StopwatchFor().Item1;

            // Assert, Then

            tempoParaTextoSemMarcador.Should().BeLessThan(tempoParaTextoComMarcador);
            tempoParaTextoVazio.Should().BeLessThan(tempoParaTextoComMarcador);
            tempoParaTextoEmBranco.Should().BeLessThan(tempoParaTextoComMarcador);
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(OutputMarkers);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IOutputMarkers));
            sut.AssertMyOwnImplementations(
                typeof(IOutputMarkers));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}