using System;
using Kli.Wrappers;
using Xunit;

namespace Tests.UnitTests.Kli.Wrappers
{
    public class TestIEnvironment: Test
    {
        [Theory]
        [InlineData(typeof(IEnvironment), "String GetEnvironmentVariable(String)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}