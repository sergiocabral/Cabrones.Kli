using System;
using Kli.i18n;
using Xunit;

namespace Tests.UnitTests.Kli.i18n
{
    public class TestILanguage: Test
    {
        [Theory]
        [InlineData(typeof(ILanguage), "IEnumerable<String> get_EnvironmentVariables()")]
        [InlineData(typeof(ILanguage), "String FromEnvironment()")]
        [InlineData(typeof(ILanguage), "String FromSystem()")]
        [InlineData(typeof(ILanguage), "String get_Current()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}