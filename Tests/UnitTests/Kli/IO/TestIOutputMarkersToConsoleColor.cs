using System;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestIOutputMarkersToConsoleColor: Test
    {
        [Theory]
        [InlineData(typeof(IOutputMarkersToConsoleColor), "Char Convert(ConsoleColor, ConsoleColor = 'Black')")]
        [InlineData(typeof(IOutputMarkersToConsoleColor), "Tuple<ConsoleColor, ConsoleColor> Convert(Char)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}