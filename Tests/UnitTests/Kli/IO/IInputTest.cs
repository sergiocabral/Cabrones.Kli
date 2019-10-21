using System.Reflection;
using FluentAssertions;
using Kli.IO;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    // ReSharper disable once InconsistentNaming
    public class IInputTest: Test
    {
        [Fact]
        public void verifica_se_a_interface_tem_os_meios_de_leitura_da_parte_do_usuario()
        {
            // Arrange, Given

            var tipoDaInterface = typeof(IInput);

            // Act, When
            
            var métodoDeLeitura = tipoDaInterface.GetMethod("Read", BindingFlags.Instance | BindingFlags.Public);

            // Assert, Then
            
            métodoDeLeitura.Should().NotBeNull();
        }
        
        [Fact]
        public void verifica_se_o_método_de_leitura_retorna_o_tipo_string()
        {
            // Arrange, Given

            var tipoDaInterface = typeof(IInput);

            // Act, When
            
            var métodoDeLeitura = tipoDaInterface.GetMethod("Read", BindingFlags.Instance | BindingFlags.Public);
            var tipoDeRetorno = métodoDeLeitura?.ReturnType;

            // Assert, Then

            tipoDeRetorno.Should().Be<string>();
        }
        
        [Fact]
        public void verifica_se_o_método_de_leitura_permite_informar_que_o_dado_é_sensível()
        {
            // Arrange, Given

            var tipoDaInterface = typeof(IInput);

            // Act, When
            
            var métodoDeLeitura = tipoDaInterface.GetMethod("Read", BindingFlags.Instance | BindingFlags.Public);
            var parâmetrosDoMétodo = métodoDeLeitura?.GetParameters() ?? Substitute.For<ParameterInfo[]>(); 

            // Assert, Then

            parâmetrosDoMétodo.Length.Should().Be(1);
            parâmetrosDoMétodo[0].ParameterType.Should().Be<bool>();
            parâmetrosDoMétodo[0].Name.ToLower().Should().Contain("sensitive");
        }
        
        [Fact]
        public void verifica_no_método_de_leitura_se_o_parâmetro_que_informa_que_o_dado_é_sensível_tem_valor_padrao()
        {
            // Arrange, Given

            var tipoDaInterface = typeof(IInput);

            // Act, When
            
            var métodoDeLeitura = tipoDaInterface.GetMethod("Read", BindingFlags.Instance | BindingFlags.Public);
            var parâmetrosDoMétodo = métodoDeLeitura?.GetParameters()[0] ?? Substitute.For<ParameterInfo>(); 

            // Assert, Then

            parâmetrosDoMétodo.IsOptional.Should().Be(true);
            parâmetrosDoMétodo.DefaultValue.Should().Be(false);
        }
    }
}