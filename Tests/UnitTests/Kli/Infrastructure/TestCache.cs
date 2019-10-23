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
        [InlineData(typeof(Cache), typeof(ICache))]
        public void verifica_se_classe_implementa_tipos(Type tipoDaClasse, Type tipoQueDeveSerImplementado)
        {
            verifica_se_classe_implementa_tipo(tipoDaClasse, tipoQueDeveSerImplementado);
        }

        [Fact]
        public void um_valor_de_cache_deve_ser_gravado_e_lido_corretamente()
        {
            // Arrange, Given

            var cache = DependencyResolverFromProgram.GetInstance<ICache>();
            var identificador = Fixture.Create<string>();
            var valorGravado = Fixture.Create<string>();

            // Act, When

            var valorRetornado = cache.Save(identificador, valorGravado);
            var valorLido = cache.Read<string>(identificador);

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

            cache.Save(identificador, valorGravado);
            cache.Save<string>(identificador, null);
            var valorLido = cache.Read<string>(identificador);

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

            var método = cache.GetType().GetMethod("Read", BindingFlags.Instance | BindingFlags.Public) ?? Substitute.For<MethodInfo>();
            var métodoComGeneric = método.MakeGenericMethod(tipo);

            var valorLido = métodoComGeneric.Invoke(cache, new object[] { identificador });

            // Assert, Then

            valorLido.Should().Be(tipo.IsValueType ? Activator.CreateInstance(tipo) : null);
        }
    }
}