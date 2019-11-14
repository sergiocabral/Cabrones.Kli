using System;
using Test;
using Xunit;

namespace Kli.Wrappers
{
    public class TestConsole: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Console), 17)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Console), typeof(IConsole))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            TestTypeImplementations(tipoDaClasse, tiposQueDeveSerImplementado);
    }
}