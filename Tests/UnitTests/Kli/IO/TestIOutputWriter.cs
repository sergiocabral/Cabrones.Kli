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
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}