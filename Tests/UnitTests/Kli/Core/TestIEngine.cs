using System;
using Kli.Core;
using Xunit;

namespace Tests.UnitTests.Kli.Core
{
    public class TestIEngine: Test
    {
        [Theory]
        [InlineData(typeof(IEngine), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);
        
        [Theory]
        [InlineData(typeof(IEngine), "Void Run()")]
        public void verifica_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}