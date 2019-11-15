using System;
using FluentAssertions;
using Kli.Infrastructure;
using NSubstitute;
using Cabrones.Test;
using Xunit;

namespace Kli.Output
{
    public class TestExtensions
    {
        [Theory]
        [InlineData(typeof(Extensions), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestTypeMethodsCount(totalDeMétodosEsperado);
        
        [Fact]
        public void verifica_se_o_resolvedor_de_dependência_da_classe_está_sendo_usado_quando_é_definido()
        {   
            // Arrange, Given
            
            var dependencyResolver = Substitute.For<IDependencyResolver>();
            
            // Act, When

            Extensions.DependencyResolver = dependencyResolver;
            
            // Assert, Then

            Extensions.DependencyResolver.Should().BeSameAs(dependencyResolver);
        }
        
        [Fact]
        public void verifica_se_o_resolvedor_de_dependência_da_classe_usa_o_valor_padrão_quando_é_definido_nulo()
        {   
            // Arrange, Given
            // Act, When

            Extensions.DependencyResolver = null;
            
            // Assert, Then

            Extensions.DependencyResolver.Should().BeSameAs(Program.DependencyResolver);
        }

        [Fact]
        public void verifica_se_método_EscapeForOutput_faz_uso_classe_OutputMarkers()
        {
            // Arrange, Given

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            Extensions.DependencyResolver = dependencyResolver;
            
            // Act, When

            var texto = string.Empty; 
            texto.EscapeForOutput();
            
            // Assert, Then

            Extensions.DependencyResolver.GetInstance<IOutputMarkers>().Received(1).Escape(texto);
        }

        [Fact]
        public void verifica_se_método_EscapeForOutput_funciona_como_na_classe_OutputMarkers()
        {
            // Arrange, Given

            Extensions.DependencyResolver = Program.DependencyResolver;
            var outputFormatter = Extensions.DependencyResolver.GetInstance<IOutputMarkers>();

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