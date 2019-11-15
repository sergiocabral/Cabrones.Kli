using System;
using Kli.Core;
using NSubstitute;
using Cabrones.Test;
using Xunit;

namespace Kli.Module
{
    public class TestMultipleModule
    {
        [Theory]
        [InlineData(typeof(MultipleModule), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestTypeMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(MultipleModule), typeof(Multiple<IModule>), typeof(IMultiple<IModule>), typeof(IMultipleModule), typeof(IModule))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            tipoDaClasse.TestTypeImplementations(tiposQueDeveSerImplementado);
        
        [Fact]
        public void o_método_Run_deve_chamar_o_serviço_IUtils_para_exibir_as_opções_inicias()
        {
            // Arrange, Given

            var utils = Substitute.For<IInteraction>();
            var multipleModule = new MultipleModule(utils);

            // Act, When
            
            multipleModule.Run();

            // Assert, Then

            utils.Received(1).StartInteraction();
        }
    }
}