using Cabrones.Test;
using FluentAssertions;
using Kli.Infrastructure;
using NSubstitute;
using Xunit;

namespace Kli.Output
{
    public class TestExtensions
    {
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
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Extensions);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(2);
            sut.AssertPublicPropertyPresence("static IDependencyResolver DependencyResolver { get; set; }");
            sut.AssertMyOwnPublicMethodsCount(1);
            sut.AssertPublicMethodPresence("static String EscapeForOutput(String)");

            sut.IsClass.Should().BeTrue();
        }
    }
}