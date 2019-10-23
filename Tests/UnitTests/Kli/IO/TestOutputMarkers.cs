using System;
using System.Diagnostics;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestOutputMarkers: Test
    {
        [Theory]
        [InlineData(typeof(OutputMarkers), typeof(IOutputMarkers))]
        public void verifica_se_classe_implementa_tipos(Type tipoDaClasse, Type tipoQueDeveSerImplementado)
        {
            verifica_se_classe_implementa_tipo(tipoDaClasse, tipoQueDeveSerImplementado);
        }
        
        [Fact]
        public void verifica_se_os_valores_dos_marcadores_estao_corretos()
        {
            // Arrange, Given

            var outputFormatter = DependencyResolverFromProgram.GetInstance<IOutputMarkers>();

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
        public void verifica_se_lista_de_marcadores_contem_todos_os_valores()
        {
            // Arrange, Given

            var outputFormatter = DependencyResolverFromProgram.GetInstance<IOutputMarkers>();

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
        public void a_consulta_da_lista_de_marcadores_deve_usar_o_cache_nas_proximas_vezes()
        {
            // Arrange, Given

            var outputFormatter = DependencyResolverFromProgram.GetInstance<IOutputMarkers>();

            Tuple<T, long, T, long> Consultar<T>(Func<T> propriedade)
            {
                var cronômetro = new Stopwatch();
                
                cronômetro.Start();
                var valores1 = propriedade();
                cronômetro.Stop();
                var tempo1 = cronômetro.ElapsedTicks;
                
                cronômetro.Restart();
                var valores2 = propriedade();
                cronômetro.Stop();
                var tempo2 = cronômetro.ElapsedTicks;
                
                return new Tuple<T, long, T, long>(valores1, tempo1, valores2, tempo2);
            }

            // Act, When

            var (markersValores1, markersTempo1, markersValores2, markersTempo2) = Consultar<string>(() => outputFormatter.Markers);
            var (markersEscapedForRegexJoinedValores1, markersEscapedForRegexJoinedTempo1, markersEscapedForRegexJoinedValores2, markersEscapedForRegexJoinedTempo2) = Consultar<string>(() => outputFormatter.MarkersEscapedForRegexJoined);
            var (markersEscapedForRegexSeparatedValores1, markersEscapedForRegexSeparatedTempo1, markersEscapedForRegexSeparatedValores2, markersEscapedForRegexSeparatedTempo2) = Consultar<string[]>(() => outputFormatter.MarkersEscapedForRegexSeparated);

            // Assert, Then

            markersValores2.Should().Be(markersValores1);
            markersTempo2.Should().BeLessThan(markersTempo1);
            
            markersEscapedForRegexJoinedValores2.Should().Be(markersEscapedForRegexJoinedValores1);
            markersEscapedForRegexJoinedTempo2.Should().BeLessThan(markersEscapedForRegexJoinedTempo1);

            markersEscapedForRegexSeparatedValores2.Should().BeEquivalentTo(markersEscapedForRegexSeparatedValores1);
            markersEscapedForRegexSeparatedTempo2.Should().BeLessThan(markersEscapedForRegexSeparatedTempo1);
        }

        [Fact]
        public void verifica_se_os_caracteres_tipo_marcadores_podem_ser_escapados()
        {
            // Arrange, Given

            var outputFormatter = DependencyResolverFromProgram.GetInstance<IOutputMarkers>();

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
        public void verifica_se_texto_vazio_ou_em_branco_ou_sem_marcadores_é_prontamente_retornada_sem_fazer_análise()
        {
            // Arrange, Given

            var outputFormatter = DependencyResolverFromProgram.GetInstance<IOutputMarkers>();

            long TempoGastoParaEscapar(string texto)
            {
                var cronômetro = new Stopwatch();
                cronômetro.Start();
                outputFormatter.Escape(texto);
                cronômetro.Stop();
                return cronômetro.ElapsedTicks;
            }
                
            // Act, When

            var tempoParaTextoComMarcador = TempoGastoParaEscapar(outputFormatter.MarkersEscapedForRegexJoined);
            var tempoParaTextoSemMarcador = TempoGastoParaEscapar(Fixture.Create<string>().Substring(0, 10));
            var tempoParaTextoVazio = TempoGastoParaEscapar(string.Empty);
            var tempoParaTextoEmBranco = TempoGastoParaEscapar("          ");
            
            // Assert, Then

            tempoParaTextoSemMarcador.Should().BeLessThan(tempoParaTextoComMarcador);
            tempoParaTextoVazio.Should().BeLessThan(tempoParaTextoComMarcador);
            tempoParaTextoEmBranco.Should().BeLessThan(tempoParaTextoComMarcador);
        }
    }
}