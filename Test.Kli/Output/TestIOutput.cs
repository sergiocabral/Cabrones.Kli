using System;
using Cabrones.Test;
using Xunit;

namespace Kli.Output
{
    public class TestIOutput
    {
        [Theory]
        [InlineData(typeof(IOutput), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IOutput), "IOutput Write(String)")]
        [InlineData(typeof(IOutput), "IOutput WriteLine(String)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            tipo.TestMethodPresence(assinaturaEsperada);
    }
}