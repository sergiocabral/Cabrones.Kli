using System;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestIInput: Test
    {
        [Theory]
        [InlineData(typeof(IInput), "String Read(Boolean = False)")]
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}