﻿using System;
using Cabrones.Test;
using Xunit;

namespace Kli.Wrappers
{
    public class TestIEnvironment
    {
        [Theory]
        [InlineData(typeof(IEnvironment), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IEnvironment), "String GetEnvironmentVariable(String)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            tipo.TestMethodPresence(assinaturaEsperada);
    }
}