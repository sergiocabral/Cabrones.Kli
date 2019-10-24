using AutoFixture;
using FluentAssertions;
using Kli;
using Kli.i18n;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.i18n
{
    public class TestExtensions: Test
    {
        [Fact]
        public void verifica_se_método_Translate_faz_uso_classe_Translate()
        {
            // Arrange, Given

            var dependencyResolver = Program.DependencyResolver = DependencyResolverForTest;

            // Act, When

            var texto = Fixture.Create<string>();
            var idioma = Fixture.Create<string>();
            texto.Translate(idioma);
            
            // Assert, Then

            dependencyResolver.GetInstance<ITranslate>().Received(1).Get(texto, idioma);
        }

        [Fact]
        public void verifica_se_método_Translate_funciona_como_na_classe_Translate()
        {
            // Arrange, Given

            var dependencyResolver = Program.DependencyResolver = DependencyResolverFromProgram;
            var tradução = dependencyResolver.GetInstance<ITranslate>();

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
    }
}