using FluentAssertions;
using Kli;
using Kli.IO;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestExtensions: Test
    {
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