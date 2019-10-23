using System;
using Kli.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class TestICache: Test
    {
        [Theory]
        [InlineData(typeof(ICache), "T Set<T>(String, T)")]
        [InlineData(typeof(ICache), "T Get<T>(String)")]
        [InlineData(typeof(ICache), "Void Clear()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}