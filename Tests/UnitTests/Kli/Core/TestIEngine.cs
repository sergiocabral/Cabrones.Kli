using System;
using Kli.Core;
using Xunit;

namespace Tests.UnitTests.Kli.Core
{
    public class TestIEngine: Test
    {
        [Theory]
        [InlineData(typeof(IEngine), "Void Run()")]
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}