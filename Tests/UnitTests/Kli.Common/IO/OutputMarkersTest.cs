using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Kli.Common.IO;
using Xunit;

namespace Tests.UnitTests.Kli.Common.IO
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

            // Act, When

            var cronômetro1 = new Stopwatch();
            cronômetro1.Start();
            var markers1 = outputFormatter.Markers;
            cronômetro1.Stop();

            var cronômetro2 = new Stopwatch();
            cronômetro2.Start();
            var markers2 = outputFormatter.Markers;
            cronômetro2.Stop();

            // Assert, Then

            markers1.Should().BeEquivalentTo(markers2);
            cronômetro2.ElapsedTicks.Should().BeLessThan(cronômetro1.ElapsedTicks);
        }
    }
}