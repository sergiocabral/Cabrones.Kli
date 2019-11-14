using System;
using Test;
using Xunit;

namespace Kli.Module
{
    public class TestIInteraction: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IInteraction), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);
        
        [Theory]
        [InlineData(typeof(IInteraction), "Void StartInteraction()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            TestTypeMethodSignature(tipo, assinaturaEsperada);
    }
}