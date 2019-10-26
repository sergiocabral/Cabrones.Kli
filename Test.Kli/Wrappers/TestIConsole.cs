using System;
using Test;
using Xunit;

namespace Kli.Wrappers
{
    public class TestIConsole: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IConsole), 7)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IConsole), "ConsoleColor get_ForegroundColor()")]
        [InlineData(typeof(IConsole), "Void set_ForegroundColor(ConsoleColor)")]
        [InlineData(typeof(IConsole), "ConsoleColor get_BackgroundColor()")]
        [InlineData(typeof(IConsole), "Void set_BackgroundColor(ConsoleColor)")]
        [InlineData(typeof(IConsole), "Void ResetColor()")]
        [InlineData(typeof(IConsole), "Void Write(String)")]
        [InlineData(typeof(IConsole), "Void WriteLine(String)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}