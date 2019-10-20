using System;
using System.Reflection;
using FluentAssertions;
using Kli.Core;
using Kli.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class DependencyResolverTest: Test
    {
        [Theory]
        [InlineData(typeof(IDependencyResolver))]
        [InlineData(typeof(IConsoleConfiguration))]
        [InlineData(typeof(IEngine))]
        public void verifica_se_o_serviço_está_sendo_resolvido(Type tipoDoServiço)
        {
            // Arrange, Given
            
            var métodoQueObtemOServiço = 
                typeof(IDependencyResolver)
                    .GetMethod("GetInstance", BindingFlags.Instance | BindingFlags.Public);
            
            var métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço =
                métodoQueObtemOServiço?.MakeGenericMethod(tipoDoServiço);
            
            // Act, When
            
            var serviço = métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço?.Invoke(DependencyResolver.Default, null);

            // Assert, Then
            
            serviço.Should().NotBeNull();
        }
    }
}