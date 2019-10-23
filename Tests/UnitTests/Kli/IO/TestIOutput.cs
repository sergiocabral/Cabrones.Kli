using System;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestIOutput: Test
    {
        [Theory]
        [InlineData(typeof(IOutput), "IOutput Write(String)")]
        [InlineData(typeof(IOutput), "IOutput WriteLine(String)")]
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}