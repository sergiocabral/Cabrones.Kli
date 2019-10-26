﻿using System;
using Test;
using Xunit;

namespace Kli.i18n
{
    public class TestILanguage: BaseForTest
    {
        [Theory]
        [InlineData(typeof(ILanguage), 4)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(ILanguage), "IEnumerable<String> get_EnvironmentVariables()")]
        [InlineData(typeof(ILanguage), "String FromEnvironment()")]
        [InlineData(typeof(ILanguage), "String FromSystem()")]
        [InlineData(typeof(ILanguage), "String get_Current()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}