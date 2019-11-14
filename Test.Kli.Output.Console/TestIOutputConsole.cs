using System;
using Test;
using Xunit;

namespace Kli.Output.Console
{
    public class TestIOutputConsole: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IOutputConsole), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IOutputConsole), "Void WriteToConsole(String, Char = '')")]
        [InlineData(typeof(IOutputConsole), "Char CurrentMarker()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            TestTypeMethodSignature(tipo, assinaturaEsperada);
    }
}