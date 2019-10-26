using System;
using Test;
using Xunit;

namespace Kli.Core
{
    public class TestILoaderAssembly: BaseForTest
    {
        [Theory]
        [InlineData(typeof(ILoaderAssembly), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);
        
        [Theory]
        [InlineData(typeof(ILoaderAssembly), "IDictionary<String, Assembly> Load(String)")]
        [InlineData(typeof(ILoaderAssembly), "IEnumerable<Type> RegisterServices(String)")]
        [InlineData(typeof(ILoaderAssembly), "IEnumerable<TService> GetInstances<TService>(String)")]
        public void verifica_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}