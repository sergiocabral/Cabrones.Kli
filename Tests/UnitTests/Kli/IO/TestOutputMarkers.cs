using System;
using System.Linq;
using FluentAssertions;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestOutputMarkers: Test
    {
        [Theory]
        [InlineData(typeof(OutputMarkers), 10)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(OutputMarkers), typeof(IOutputMarkers))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);
        
        [Theory]
        [InlineData(typeof(IOutputMarkers), "Markers")]
        [InlineData(typeof(IOutputMarkers), "MarkersEscapedForRegexJoined")]
        [InlineData(typeof(IOutputMarkers), "MarkersEscapedForRegexSeparated")]
        public void verifica_se_o_cache_está_sendo_usado_nas_consultas(Type tipo, string nomeDePropriedade) =>
            verifica_se_o_cache_está_sendo_usado_na_consulta(tipo, nomeDePropriedade);
        
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
                
            // Act, When

            var tempoParaTextoComMarcador = cronometrar_consulta(() => outputFormatter.Escape(outputFormatter.MarkersEscapedForRegexJoined)).Item1;
            var tempoParaTextoSemMarcador = cronometrar_consulta(() => outputFormatter.Escape("12345")).Item1;
            var tempoParaTextoVazio = cronometrar_consulta(() => outputFormatter.Escape(string.Empty)).Item1;
            var tempoParaTextoEmBranco = cronometrar_consulta(() => outputFormatter.Escape("     ")).Item1;
            
            // Assert, Then

            tempoParaTextoSemMarcador.Should().BeLessThan(tempoParaTextoComMarcador);
            tempoParaTextoVazio.Should().BeLessThan(tempoParaTextoComMarcador);
            tempoParaTextoEmBranco.Should().BeLessThan(tempoParaTextoComMarcador);
        }
    }
}