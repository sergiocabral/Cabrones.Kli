using System;
using FluentAssertions;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Output
{
    public class TestExtensions: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Extensions), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Fact]
        public void verifica_se_método_EscapeForOutput_faz_uso_classe_OutputMarkers()
        {
            // Arrange, Given

            var dependencyResolver = Program.DependencyResolver = DependencyResolverForTest;

            // Act, When

            var texto = string.Empty; 
            texto.EscapeForOutput();
            
            // Assert, Then

            dependencyResolver.GetInstance<IOutputMarkers>().Received(1).Escape(texto);
        }

        [Fact]
        public void verifica_se_método_EscapeForOutput_funciona_como_na_classe_OutputMarkers()
        {
            // Arrange, Given

            var dependencyResolver = Program.DependencyResolver = DependencyResolverFromProgram;
            var outputFormatter = dependencyResolver.GetInstance<IOutputMarkers>();

            foreach (var marcador in outputFormatter.Markers)
            {

                var texto = $"marcador: {marcador}.";

                // Act, When

                var textoEscapadoPelaClasse = outputFormatter.Escape(texto);
                var textoEscapadoPelaExtension = texto.EscapeForOutput();

                // Assert, Then

                textoEscapadoPelaExtension.Should().Be(textoEscapadoPelaClasse);
            }
        }        
    }
}