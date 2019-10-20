using System;
using System.Reflection;
using FluentAssertions;
using Kli.Core;
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
        
        [Theory]
        [InlineData(typeof(IConsoleConfiguration))]
        [InlineData(typeof(IEngine))]
        public void verifica_se_o_serviço_está_sendo_resolvido(Type tipoDoServiço)
        {
            // Arrange, Given
            
            var métodoQueObtemOServiço =
                typeof(DependencyResolver)
                    .GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public);
            
            var métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço =
                métodoQueObtemOServiço?.MakeGenericMethod(tipoDoServiço);
            
            // Act, When
            
            var serviço = métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço?.Invoke(null, null);

            // Assert, Then
            
            serviço.Should().NotBeNull();
        }
    }
}