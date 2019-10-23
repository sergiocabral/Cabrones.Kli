using System;
using Kli;
using Kli.Core;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli
{
    public class ProgramTest: Test
    {
        [Theory]
        [InlineData(typeof(Program), "IDependencyResolver get_DependencyResolver()")]
        [InlineData(typeof(Program), "Void Main()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
        
        [Fact]
        public void verifica_se_o_programa_chama_a_classe_com_a_lógica_principal()
        {
            // Arrange, Given
            
            var dependencyResolver = Program.DependencyResolver = DependencyResolverForTest;

            // Act, When
            
            Program.Main();

            // Assert, Then

            dependencyResolver.GetInstance<IEngine>().Received(1).Run();
        }
    }
}