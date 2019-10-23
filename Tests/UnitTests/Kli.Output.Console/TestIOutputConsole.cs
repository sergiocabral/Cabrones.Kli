using System;
using Kli.Output.Console;
using Xunit;

namespace Tests.UnitTests.Kli.Output.Console
{
    public class TestIOutputConsole: Test
    {
        [Theory]
        [InlineData(typeof(IOutputConsole), "Void WriteToConsole(String, Char = 0)")]
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}