using System;
using System.Reflection;
using AutoFixture;
using FluentAssertions;
using Kli.Infrastructure;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class TestCache: Test
    {
        [Theory]
        [InlineData(typeof(Cache), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Cache), typeof(ICache))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

        [Fact]
        public void um_valor_de_cache_deve_ser_gravado_e_lido_corretamente()
        {
            // Arrange, Given

            var cache = DependencyResolverFromProgram.GetInstance<ICache>();
            var identificador = Fixture.Create<string>();
            var valorGravado = Fixture.Create<string>();

            // Act, When

            var valorRetornado = cache.Set(identificador, valorGravado);
            var valorLido = cache.Get<string>(identificador);

            // Assert, Then
            
            valorRetornado.Should().BeSameAs(valorGravado);
            valorLido.Should().BeSameAs(valorGravado);
        }

        [Fact]
        public void um_valor_de_cache_é_removido_definido_null_como_valor()
        {
            // Arrange, Given

            var cache = DependencyResolverFromProgram.GetInstance<ICache>();
            var identificador = Fixture.Create<string>();
            var valorGravado = Fixture.Create<string>();

            // Act, When

            cache.Set(identificador, valorGravado);
            cache.Set<string>(identificador, null);
            var valorLido = cache.Get<string>(identificador);

            // Assert, Then

            valorLido.Should().BeNull();
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(int))]
        [InlineData(typeof(object))]
        [InlineData(typeof(float[]))]
        public void ler_um_valor_que_não_existe_deve_retornar_o_valor_default(Type tipo)
        {
            // Arrange, Given

            var cache = DependencyResolverFromProgram.GetInstance<ICache>();
            var identificador = Fixture.Create<string>();

            // Act, When

            var método = cache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.Public) ?? Substitute.For<MethodInfo>();
            var métodoComGeneric = método.MakeGenericMethod(tipo);

            var valorLido = métodoComGeneric.Invoke(cache, new object[] { identificador });

            // Assert, Then

            valorLido.Should().Be(tipo.IsValueType ? Activator.CreateInstance(tipo) : null);
        }
        
        [Fact]
        public void verifica_se_cache_está_sendo_limpo()
        {
            // Arrange, Given

            var cache = DependencyResolverFromProgram.GetInstance<ICache>();
            var identificador = Fixture.Create<string>();
            var valor = Fixture.Create<string>();

            // Act, When

            cache.Set(identificador, valor);
            cache.Clear();
            var valorLido = cache.Get<string>(identificador);

            // Assert, Then

            valorLido.Should().BeNull();
        }
    }
}