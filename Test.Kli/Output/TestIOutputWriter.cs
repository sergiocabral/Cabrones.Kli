using System;
using Cabrones.Test;
using Xunit;

namespace Kli.Output
{
    public class TestIOutputWriter
    {
        [Theory]
        [InlineData(typeof(IOutputWriter), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestTypeMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(IOutputWriter), "Void Parse(String, Action<String, Char>)")]
        [InlineData(typeof(IOutputWriter), "Void Parse(String, Action<String>)")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            tipo.TestTypeMethodSignature(assinaturaEsperada);
    }
}