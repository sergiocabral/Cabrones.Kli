using System;
using Kli.Output.Console;
using Xunit;

namespace Tests.UnitTests.Kli.Output.Console
{
    public class TestIOutputConsole: Test
    {
        [Theory]
        [InlineData(typeof(IOutputConsole), "Void WriteToConsole(String, Char = 0)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}