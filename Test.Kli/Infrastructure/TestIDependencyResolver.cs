using System;
using Test;
using Xunit;

namespace Kli.Infrastructure
{
    public class TestIDependencyResolver: BaseForTest
    {
        [Theory]
        [InlineData(typeof(IDependencyResolver), 7)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);
        
        [Theory]
        [InlineData(typeof(IDependencyResolver), "Guid CreateScope(Nullable<Guid> = null)")]
        [InlineData(typeof(IDependencyResolver), "Void DisposeScope(Guid)")]
        [InlineData(typeof(IDependencyResolver), "Boolean IsActive(Guid)")]
        [InlineData(typeof(IDependencyResolver), "TService GetInstance<TService>(Nullable<Guid> = null)")]
        [InlineData(typeof(IDependencyResolver), "Object GetInstance(Type, Nullable<Guid> = null)")]
        [InlineData(typeof(IDependencyResolver), "Void Register<TService, TImplementation>(DependencyResolverLifeTime = 'PerContainer')")]
        [InlineData(typeof(IDependencyResolver), "Void Register(Type, Type, DependencyResolverLifeTime = 'PerContainer')")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}