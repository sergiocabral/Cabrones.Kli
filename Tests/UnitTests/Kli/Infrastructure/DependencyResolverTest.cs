using System;
using System.Reflection;
using FluentAssertions;
using Kli.Common.IO;
using Kli.Core;
using Kli.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class DependencyResolverTest: Test
    {
        [Theory]
        [InlineData(typeof(IOutputMarkers))]
        [InlineData(typeof(IConsoleConfiguration))]
        [InlineData(typeof(IEngine))]
        [InlineData(typeof(IDependencyResolver))]
        public void verifica_se_o_serviço_está_sendo_resolvido(Type tipoDoServiço)
        {
            // Arrange, Given
            
            var métodoQueObtemOServiço = 
                typeof(IDependencyResolver)
                    .GetMethod("GetInstance", BindingFlags.Instance | BindingFlags.Public);
            
            var métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço =
                métodoQueObtemOServiço?.MakeGenericMethod(tipoDoServiço);
            
            // Act, When
            
            var serviço = métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço?.Invoke(DependencyResolverFromProgram, null);

            // Assert, Then
            
            serviço.Should().NotBeNull();
        }
    }
}