using System.Reflection;
using FluentAssertions;
using Kli.Common.IO;
using Xunit;

namespace Tests.UnitTests.Kli.Common.IO
{
    // ReSharper disable once InconsistentNaming
    public class IOutputTest: Test
    {
        [Fact]
        public void verifica_se_a_interface_tem_os_meios_de_escrita_para_o_usuario()
        {
            // Arrange, Given

            var tipoDaInterface = typeof(IOutput);

            // Act, When
            
            var métodoDeEscrita = tipoDaInterface.GetMethod("Write", BindingFlags.Instance | BindingFlags.Public);
            var métodoDeEscritaComQuebraDeLinha = tipoDaInterface.GetMethod("WriteLine", BindingFlags.Instance | BindingFlags.Public);

            // Assert, Then
            
            métodoDeEscrita.Should().NotBeNull();
            métodoDeEscritaComQuebraDeLinha.Should().NotBeNull();
        }
        
        [Fact]
        public void verifica_se_os_métodos_de_escrita_retornam_a_propria_instance()
        {
            // Arrange, Given

            var tipoDaInterface = typeof(IOutput);

            // Act, When
            
            var métodoDeEscrita = tipoDaInterface.GetMethod("Write", BindingFlags.Instance | BindingFlags.Public);
            var métodoDeEscritaComQuebraDeLinha = tipoDaInterface.GetMethod("WriteLine", BindingFlags.Instance | BindingFlags.Public);

            var tipoDeRetornoDoMétodoDeEscrita = métodoDeEscrita?.ReturnType;
            var tipoDeRetornoDoMétodoDeEscritaComQuebraDeLinha = métodoDeEscritaComQuebraDeLinha?.ReturnType;

            // Assert, Then

            tipoDeRetornoDoMétodoDeEscrita.Should().Be<IOutput>();
            tipoDeRetornoDoMétodoDeEscritaComQuebraDeLinha.Should().Be<IOutput>();
        }
        
        [Fact]
        public void verifica_se_os_métodos_de_escrita_recebem_como_parametro_string()
        {
            // Arrange, Given

            var tipoDaInterface = typeof(IOutput);

            // Act, When
            
            var métodoDeEscrita = tipoDaInterface.GetMethod("Write", BindingFlags.Instance | BindingFlags.Public);
            var métodoDeEscritaComQuebraDeLinha = tipoDaInterface.GetMethod("WriteLine", BindingFlags.Instance | BindingFlags.Public);
            
            var parâmetrosDoMétodoDeEscrita = métodoDeEscrita?.GetParameters() ?? new ParameterInfo[] { };
            var parâmetrosDoMétodoDeEscritaComQuebraDeLinha = métodoDeEscritaComQuebraDeLinha?.GetParameters() ?? new ParameterInfo[] { };

            // Assert, Then

            parâmetrosDoMétodoDeEscrita.Length.Should().Be(1);
            parâmetrosDoMétodoDeEscrita[0].ParameterType.Should().Be<string>();
            
            parâmetrosDoMétodoDeEscritaComQuebraDeLinha.Length.Should().Be(1);
            parâmetrosDoMétodoDeEscritaComQuebraDeLinha[0].ParameterType.Should().Be<string>();
        }
    }
}