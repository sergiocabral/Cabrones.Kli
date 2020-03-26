using System;
using System.Reflection;
using Cabrones.Test;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Kli.Infrastructure
{
    public class TestCache
    {
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

            var cache = Program.DependencyResolver.GetInstance<ICache>();
            var identificador = this.Fixture<string>();

            // Act, When

            var método = cache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.Public) ??
                         Substitute.For<MethodInfo>();
            var métodoComGeneric = método.MakeGenericMethod(tipo);

            var valorLido = métodoComGeneric.Invoke(cache, new object[] {identificador});

            // Assert, Then

            valorLido.Should().Be(tipo.IsValueType ? Activator.CreateInstance(tipo) : null);
        }

        [Fact]
        public void um_valor_de_cache_deve_ser_gravado_e_lido_corretamente()
        {
            // Arrange, Given

            var cache = Program.DependencyResolver.GetInstance<ICache>();
            var identificador = this.Fixture<string>();
            var valorGravado = this.Fixture<string>();

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

            var cache = Program.DependencyResolver.GetInstance<ICache>();
            var identificador = this.Fixture<string>();
            var valorGravado = this.Fixture<string>();

            // Act, When

            cache.Set(identificador, valorGravado);
            cache.Set<string>(identificador, null);
            var valorLido = cache.Get<string>(identificador);

            // Assert, Then

            valorLido.Should().BeNull();
        }

        [Fact]
        public void verifica_se_cache_está_sendo_limpo()
        {
            // Arrange, Given

            var cache = Program.DependencyResolver.GetInstance<ICache>();
            var identificador = this.Fixture<string>();
            var valor = this.Fixture<string>();

            // Act, When

            cache.Set(identificador, valor);
            cache.Clear();
            var valorLido = cache.Get<string>(identificador);

            // Assert, Then

            valorLido.Should().BeNull();
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Cache);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(ICache));
            sut.AssertMyOwnImplementations(
                typeof(ICache));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}