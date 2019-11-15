using System;
using Cabrones.Test;
using Xunit;

namespace Kli.i18n
{
    public class TestITranslate
    {
        [Theory]
        [InlineData(typeof(ITranslate), 8)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(ITranslate), "String get_LanguageDefault()")]
        [InlineData(typeof(ITranslate), "Void set_LanguageDefault(String)")]
        [InlineData(typeof(ITranslate), "String Get(String, String = null)")]
        [InlineData(typeof(ITranslate), "IDictionary<String, IDictionary<String, String>> get_Translates()")]
        [InlineData(typeof(ITranslate), "Void Clear()")]
        [InlineData(typeof(ITranslate), "IDictionary<String, IDictionary<String, String>> LoadFromDictionary(IDictionary<String, IDictionary<String, String>>)")]
        [InlineData(typeof(ITranslate), "IDictionary<String, IDictionary<String, String>> LoadFromText(String)")]
        [InlineData(typeof(ITranslate), "IDictionary<String, IDictionary<String, String>> LoadFromResource(Assembly, String)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            tipo.TestMethodPresence(assinaturaEsperada);
    }
}