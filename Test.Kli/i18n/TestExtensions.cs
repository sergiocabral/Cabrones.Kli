using Cabrones.Test;
using FluentAssertions;
using Kli.Infrastructure;
using NSubstitute;
using Xunit;

namespace Kli.i18n
{
    public class TestExtensions
    {
        [Fact]
        public void verifica_se_método_Translate_faz_uso_classe_Translate()
        {
            // Arrange, Given

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            Extensions.DependencyResolver = dependencyResolver;

            // Act, When

            var texto = this.Fixture<string>();
            var idioma = this.Fixture<string>();
            texto.Translate(idioma);

            // Assert, Then

            Extensions.DependencyResolver.GetInstance<ITranslate>().Received(1).Get(texto, idioma);
        }

        [Fact]
        public void verifica_se_método_Translate_funciona_como_na_classe_Translate()
        {
            // Arrange, Given

            Extensions.DependencyResolver = Program.DependencyResolver;
            var tradução = Extensions.DependencyResolver.GetInstance<ITranslate>();

            const string texto = "Yes";
            const string idioma = "pt";

            // Act, When

            var textoTraduzidoPelaClasseSemInformarOIdioma = tradução.Get(texto, idioma);
            var textoTraduzidoPelaExtensionSemInformarOIdioma = texto.Translate(idioma);
            var textoTraduzidoPelaClasseInformandoOIdioma = tradução.Get(texto);
            var textoTraduzidoPelaExtensionInformandoOIdioma = texto.Translate();

            // Assert, Then

            textoTraduzidoPelaExtensionSemInformarOIdioma.Should().Be(textoTraduzidoPelaClasseSemInformarOIdioma);
            textoTraduzidoPelaExtensionInformandoOIdioma.Should().Be(textoTraduzidoPelaClasseInformandoOIdioma);
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
            sut.AssertPublicMethodPresence("static String Translate(String, String = null)");

            sut.IsClass.Should().BeTrue();
        }
    }
}