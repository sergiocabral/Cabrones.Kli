using System;
using Cabrones.Test;
using Xunit;

namespace Kli.Wrappers
{
    public class TestIConsole
    {
        [Theory]
        [InlineData(typeof(IConsole), 17)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestTypeMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IConsole), "ConsoleColor get_ForegroundColor()")]
        [InlineData(typeof(IConsole), "Void set_ForegroundColor(ConsoleColor)")]
        [InlineData(typeof(IConsole), "ConsoleColor get_BackgroundColor()")]
        [InlineData(typeof(IConsole), "Void set_BackgroundColor(ConsoleColor)")]
        [InlineData(typeof(IConsole), "Void ResetColor()")]
        [InlineData(typeof(IConsole), "Void Write(String)")]
        [InlineData(typeof(IConsole), "Void WriteLine(String)")]
        [InlineData(typeof(IConsole), "String ReadLine()")]
        [InlineData(typeof(IConsole), "ConsoleKeyInfo ReadKey()")]
        [InlineData(typeof(IConsole), "Boolean get_KeyAvailable()")]
        [InlineData(typeof(IConsole), "Int32 get_CursorTop()")]
        [InlineData(typeof(IConsole), "Void set_CursorTop(Int32)")]
        [InlineData(typeof(IConsole), "Int32 get_CursorLeft()")]
        [InlineData(typeof(IConsole), "Void set_CursorLeft(Int32)")]
        [InlineData(typeof(IConsole), "Int32 get_BufferHeight()")]
        [InlineData(typeof(IConsole), "Int32 get_BufferWidth()")]
        [InlineData(typeof(IConsole), "Void SetCursorPosition(Int32, Int32)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            tipo.TestTypeMethodSignature(assinaturaEsperada);
    }
}