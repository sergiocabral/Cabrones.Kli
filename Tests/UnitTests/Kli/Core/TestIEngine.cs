using System;
using Kli.Core;
using Xunit;

namespace Tests.UnitTests.Kli.Core
{
    public class TestIEngine: Test
    {
        [Theory]
        [InlineData(typeof(IEngine), "Void Run()")]
        public void verifica_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}