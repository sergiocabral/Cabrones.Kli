﻿using System;
using Test;
using Xunit;

namespace Kli.Output.File
{
    public class TestIOutputFile: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IOutputFile), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IOutputFile), "String get_Path()")]
        [InlineData(typeof(IOutputFile), "Void WriteToFile(String)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}