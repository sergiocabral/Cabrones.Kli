using System;
using Kli.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class TestICache: Test
    {
        [Theory]
        [InlineData(typeof(ICache), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(ICache), "T Set<T>(String, T)")]
        [InlineData(typeof(ICache), "T Get<T>(String)")]
        [InlineData(typeof(ICache), "Void Clear()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}