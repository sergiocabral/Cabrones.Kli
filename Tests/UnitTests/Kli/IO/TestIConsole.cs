using System;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
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
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}