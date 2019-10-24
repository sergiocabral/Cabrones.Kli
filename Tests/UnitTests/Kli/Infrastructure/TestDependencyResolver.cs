using System;
using System.Reflection;
using FluentAssertions;
using Kli.Core;
using Kli.i18n;
using Kli.Infrastructure;
using Kli.IO;
using Kli.Wrappers;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class TestDependencyResolver: Test
    {
        [Theory]
        [InlineData(typeof(DependencyResolver), typeof(IDependencyResolver))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, Type tipoQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tipoQueDeveSerImplementado);
        
        [Theory]
        [InlineData(typeof(IOutputWriter))]
        [InlineData(typeof(IOutputMarkers))]
        [InlineData(typeof(IEngine))]
        [InlineData(typeof(ICache))]
        [InlineData(typeof(IConsole))]
        [InlineData(typeof(IEnvironment))]
        [InlineData(typeof(ILanguage))]
        [InlineData(typeof(ITranslate))]
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