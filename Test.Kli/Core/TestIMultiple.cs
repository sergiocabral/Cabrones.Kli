using System;
using Kli.Module;
using Test;
using Xunit;

namespace Kli.Core
{
    public class TestIMultiple: BaseForTest
    {
        /// Este teste tem como sistema sob teste uma classe abstrata genérica.
        /// O tipo genérico foi escolhido aleatoriamente como IModule.
        /// Mas poderia ser qualquer um: IModule, IOutput ou IInput.
        
        [Theory]
        [InlineData(typeof(IMultiple<IModule>), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IMultiple<IModule>), "IList<IModule> get_Instances()")]
        [InlineData(typeof(IMultiple<IModule>), "Void Add(IModule)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}