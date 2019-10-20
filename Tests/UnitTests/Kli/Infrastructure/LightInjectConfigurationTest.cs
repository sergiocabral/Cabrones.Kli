using System;
using System.Reflection;
using FluentAssertions;
using Kli.Infrastructure;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class LightInjectConfigurationTest: Test
    {
        
        [Fact]
        public void deve_existir_o_método_capaz_de_resolver_serviços()
        {
            // Arrange, Given
            
            var tipo = typeof(DependencyResolver);

            // Act, When
            
            var método = tipo.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public);
            var parâmetrosTipoGeneric = método?.GetGenericArguments() ?? Substitute.For<Type[]>();

            // Assert, Then
            
            método.Should().NotBeNull();
            parâmetrosTipoGeneric.Length.Should().Be(1);
        }
    }
}