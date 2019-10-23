using System;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestIOutputMarkersToConsoleColor: Test
    {
        [Theory]
        [InlineData(typeof(IOutputMarkersToConsoleColor), "Char Convert(ConsoleColor, ConsoleColor = Black)")]
        [InlineData(typeof(IOutputMarkersToConsoleColor), "Tuple<ConsoleColor, ConsoleColor> Convert(Char)")]
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }
    }
}