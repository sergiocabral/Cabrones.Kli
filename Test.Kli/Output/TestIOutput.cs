using System;
using Test;
using Xunit;

namespace Kli.Output
{
    public class TestIOutput: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IOutput), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IOutput), "IOutput Write(String)")]
        [InlineData(typeof(IOutput), "IOutput WriteLine(String)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}