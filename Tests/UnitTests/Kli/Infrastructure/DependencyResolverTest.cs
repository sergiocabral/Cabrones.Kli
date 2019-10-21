using System;
using System.Reflection;
using FluentAssertions;
using Kli.Core;
using Kli.Infrastructure;
using Kli.IO;
using LightInject;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class DependencyResolverTest: Test
    {
        [Theory]
        [InlineData(typeof(IOutputWriter))]
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
        
        [Theory]
        [InlineData(typeof(IOutputWriter), typeof(PerContainerLifetime))]
        [InlineData(typeof(IOutputMarkers), typeof(PerContainerLifetime))]
        [InlineData(typeof(IConsoleConfiguration), typeof(PerContainerLifetime))]
        [InlineData(typeof(IEngine), typeof(PerContainerLifetime))]
        [InlineData(typeof(IDependencyResolver), typeof(PerContainerLifetime))]
        public void verifica_se_o_serviço_está_configurado_com_o_tempo_de_vida_correto(Type tipoDoServiço, Type tempoDeVida)
        {
            // Arrange, Given
            
            var métodoQueObtemOServiço = 
                typeof(IDependencyResolver)
                    .GetMethod("GetInstance", BindingFlags.Instance | BindingFlags.Public);
            
            var métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço =
                métodoQueObtemOServiço?.MakeGenericMethod(tipoDoServiço);
            
            // Act, When
            
            var serviço1 = métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço?.Invoke(DependencyResolverFromProgram, null);
            var serviço2 = métodoQueObtemOServiçoEspecializadoParaOTipoDoServiço?.Invoke(DependencyResolverFromProgram, null);

            // Assert, Then

            tempoDeVida.Should().Be(typeof(PerContainerLifetime));
            serviço1.Should().BeSameAs(serviço2);
        }
    }
}