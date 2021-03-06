﻿using System;
using System.Globalization;
using Cabrones.Test;
using FluentAssertions;
using Kli.Infrastructure;
using Kli.Wrappers;
using NSubstitute;
using Xunit;

namespace Kli.i18n
{
    public class TestLanguage
    {
        [Theory]
        [InlineData(typeof(ILanguage), "EnvironmentVariables")]
        [InlineData(typeof(ILanguage), "Current")]
        public void verifica_se_o_cache_está_sendo_usado_nas_consultas(Type tipo, string nomeDePropriedade)
        {
            Program.DependencyResolver.GetInstance(tipo).AssertTheSameValueButTheSecondTimeIsFaster(nomeDePropriedade);
        }

        [Theory]
        [InlineData("pt", "pt")]
        [InlineData("PT", "pt")]
        [InlineData("pT", "pt")]
        [InlineData("pt-BR", "pt")]
        [InlineData("en", "en")]
        [InlineData("EN", "en")]
        [InlineData("En", "en")]
        [InlineData("en-US", "en")]
        public void o_valor_do_idioma_na_variável_de_ambiente_deve_ser_normalizado_para_um_valor_válido(
            string valorDaVariávelDeAmbiente, string valorNormalizadoEsperado)
        {
            // Arrange, Given

            var environment = Substitute.For<IEnvironment>();
            var idioma = new Language(Substitute.For<ICache>(), environment) as ILanguage;

            environment.GetEnvironmentVariable(null).ReturnsForAnyArgs(valorDaVariávelDeAmbiente);

            // Act, When

            var idiomaPelaVariávelDeAmbiente = idioma.FromEnvironment();

            // Assert, Then

            idiomaPelaVariávelDeAmbiente.Should().Be(valorNormalizadoEsperado);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("a b c d e f")]
        [InlineData("000")]
        public void
            o_valor_do_idioma_atual_deve_ser_o_mesmo_do_sistema_operacional_quando_a_variável_de_ambiente_tem_valor_inválido(
                string exemploDeValorInválido)
        {
            // Arrange, Given

            var environment = Substitute.For<IEnvironment>();
            var cache = Program.DependencyResolver.GetInstance<ICache>();
            var idioma = new Language(cache, environment) as ILanguage;

            cache.Clear();
            environment.GetEnvironmentVariable(null).ReturnsForAnyArgs(exemploDeValorInválido);

            // Act, When

            var idiomaCorrente = idioma.Current;

            // Assert, Then

            idiomaCorrente.Should().Be(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        }

        [Theory]
        [InlineData("pt", "pt")]
        [InlineData("PT", "pt")]
        [InlineData("pT", "pt")]
        [InlineData("pt-BR", "pt")]
        [InlineData("en", "en")]
        [InlineData("EN", "en")]
        [InlineData("En", "en")]
        [InlineData("en-US", "en")]
        public void o_valor_do_idioma_atual_deve_ser_o_mesmo_da_variável_de_ambiente_se_ela_estiver_definida(
            string valorDaVariávelDeAmbiente, string idiomaCorrenteEsperado)
        {
            // Arrange, Given

            var environment = Substitute.For<IEnvironment>();
            var cache = Program.DependencyResolver.GetInstance<ICache>();
            var idioma = new Language(cache, environment) as ILanguage;

            cache.Clear();
            environment.GetEnvironmentVariable(null).ReturnsForAnyArgs(valorDaVariávelDeAmbiente);

            // Act, When

            var idiomaCorrente = idioma.Current;

            // Assert, Then

            idiomaCorrente.Should().Be(idiomaCorrenteEsperado);
        }

        [Fact]
        public void a_consulta_da_variável_de_ambiente_deve_ser_através_do_serviço_IEnvironment()
        {
            // Arrange, Given

            var environment = Substitute.For<IEnvironment>();
            var idioma = new Language(Substitute.For<ICache>(), environment) as ILanguage;

            // Act, When

            idioma.FromEnvironment();

            // Assert, Then

            environment.ReceivedWithAnyArgs().GetEnvironmentVariable(null);
        }

        [Fact]
        public void confere_os_valores_válidos_para_variáveis_de_ambiente_definirem_o_idioma()
        {
            // Arrange, Given

            var idioma = Program.DependencyResolver.GetInstance<ILanguage>();

            // Act, When

            var variáveisDeAmbientePossíveis = idioma.EnvironmentVariables;

            // Assert, Then

            variáveisDeAmbientePossíveis.Should().BeEquivalentTo("KLI-LANG", "KLI_LANG");
        }

        [Fact]
        public void
            o_valor_do_idioma_atual_deve_ser_o_mesmo_do_sistema_operacional_quando_não_definido_pela_variável_de_ambiente()
        {
            // Arrange, Given

            var environment = Substitute.For<IEnvironment>();
            var cache = Program.DependencyResolver.GetInstance<ICache>();
            var idioma = new Language(cache, environment) as ILanguage;

            cache.Clear();
            environment.GetEnvironmentVariable(null).ReturnsForAnyArgs(info => null);

            // Act, When

            var idiomaCorrente = idioma.Current;

            // Assert, Then

            idiomaCorrente.Should().Be(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        }

        [Fact]
        public void o_valor_do_idioma_do_sistema_deve_ser_o_da_interface_gráfica_do_sistema_operacional()
        {
            // Arrange, Given

            var idioma = Program.DependencyResolver.GetInstance<ILanguage>();

            // Act, When

            var nomeDaCulturaDoSistema = idioma.FromSystem();

            // Assert, Then

            nomeDaCulturaDoSistema.Should().Be(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        }

        [Fact]
        public void o_valor_do_idioma_na_variável_de_ambiente_deve_ser_nulo_quando_ela_não_é_definida()
        {
            // Arrange, Given

            var environment = Substitute.For<IEnvironment>();
            var idioma = new Language(Substitute.For<ICache>(), environment) as ILanguage;

            environment.GetEnvironmentVariable(null).ReturnsForAnyArgs(info => null);

            // Act, When

            var idiomaPelaVariávelDeAmbiente = idioma.FromEnvironment();

            // Assert, Then

            idiomaPelaVariávelDeAmbiente.Should().BeNull();
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Language);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(ILanguage));
            sut.AssertMyOwnImplementations(
                typeof(ILanguage));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}