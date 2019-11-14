using System;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.i18n
{
    public class TestExtensions: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Extensions), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);
        
        [Fact]
        public void verifica_se_o_resolvedor_de_dependência_da_classe_está_sendo_usado_quando_é_definido()
        {   
            // Arrange, Given
            // Act, When

            Extensions.DependencyResolver = DependencyResolverForTest;
            
            // Assert, Then

            Extensions.DependencyResolver.Should().BeSameAs(DependencyResolverForTest);
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
        public void verifica_se_método_Translate_faz_uso_classe_Translate()
        {
            // Arrange, Given

            Extensions.DependencyResolver = DependencyResolverForTest;

            // Act, When

            var texto = Fixture.Create<string>();
            var idioma = Fixture.Create<string>();
            texto.Translate(idioma);
            
            // Assert, Then

            Extensions.DependencyResolver.GetInstance<ITranslate>().Received(1).Get(texto, idioma);
        }

        [Fact]
        public void verifica_se_método_Translate_funciona_como_na_classe_Translate()
        {
            // Arrange, Given

            Extensions.DependencyResolver = DependencyResolverFromProgram;
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
    }
}