using System;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestIOutputWriter: Test
    {
        [Theory]
        [InlineData(typeof(IOutputWriter), "Void Parse(String, Action<String, Char>)")]
        [InlineData(typeof(IOutputWriter), "Void Parse(String, Action<String>)")]
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}