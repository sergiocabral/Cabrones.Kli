﻿using System;
using Test;
using Xunit;

namespace Kli.Input
{
    public class TestIMultipleInput: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IMultipleInput), 0)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);
    }
}