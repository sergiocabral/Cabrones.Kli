using System;
using Test;
using Xunit;

namespace Kli.Output
{
    public class TestIMultipleOutput: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IMultipleOutput), 0)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);
    }
}