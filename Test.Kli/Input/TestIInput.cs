using System;
using Cabrones.Test;
using Xunit;

namespace Kli.Input
{
    public class TestIInput
    {
        [Theory]
        [InlineData(typeof(IInput), 4)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestTypeMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IInput), "String Read(Boolean = 'False')")]
        [InlineData(typeof(IInput), "String ReadKey()")]
        [InlineData(typeof(IInput), "Boolean HasRead()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            tipo.TestTypeMethodSignature(assinaturaEsperada);
    }
}