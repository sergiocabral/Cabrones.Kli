﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class OutputMarkersTest: Test
    {
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

            KeyValuePair<IEnumerable<char>, long> Consultar()
            {
                var cronômetro = new Stopwatch();
                cronômetro.Start();
                var valores = outputFormatter.Markers;
                cronômetro.Stop();
                return new KeyValuePair<IEnumerable<char>, long>(valores, cronômetro.ElapsedTicks);
            }

            // Act, When

            var (valoresDaPrimeiraChamada, tempoDaPrimeiraChamada) = Consultar();
            var (valoresDaSegundaChamada, tempoDaSegundaChamada) = Consultar();

            // Assert, Then

            valoresDaPrimeiraChamada.Should().BeEquivalentTo(valoresDaSegundaChamada);
            tempoDaSegundaChamada.Should().BeLessThan(tempoDaPrimeiraChamada);
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
    }
}