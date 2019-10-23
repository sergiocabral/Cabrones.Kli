using System;
using Kli.Wrappers;
using Xunit;

namespace Tests.UnitTests.Kli.Wrappers
{
    public class TestIConsole: Test
    {
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