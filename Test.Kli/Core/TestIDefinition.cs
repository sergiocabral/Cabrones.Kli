using System;
using Test;
using Xunit;

namespace Kli.Core
{
    public class TestIDefinition: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IDefinition), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);
        
        [Theory]
        [InlineData(typeof(IDefinition), "String get_DirectoryOfProgram()")]
        [InlineData(typeof(IDefinition), "String get_DirectoryOfUser()")]
        [InlineData(typeof(IDefinition), "Boolean get_CanWriteIntoDirectoryOfUser()")]
        public void verifica_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}