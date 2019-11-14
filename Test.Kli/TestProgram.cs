using System;
using Kli.Core;
using NSubstitute;
using Test;
using Xunit;

namespace Kli
{
    public class TestProgram: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Program), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Program), "IDependencyResolver get_DependencyResolver()")]
        [InlineData(typeof(Program), "Void Main()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            TestTypeMethodSignature(tipo, assinaturaEsperada);
        
        [Fact]
        public void verifica_se_o_programa_chama_a_classe_com_a_lógica_principal()
        {
            // Arrange, Given
            
            var dependencyResolver = Program.DependencyResolver = DependencyResolverForTest;

            // Act, When
            
            Program.Main();

            // Assert, Then

            dependencyResolver.GetInstance<IEngine>().Received(1).Initialize();
        }
    }
}