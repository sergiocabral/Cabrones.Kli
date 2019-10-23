using System;
using Kli.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class TestICache: Test
    {
        [Theory]
        [InlineData(typeof(ICache), "T Save<T>(String, T)")]
        [InlineData(typeof(ICache), "T Read<T>(String)")]
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}