using System;
using Kli.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Kli.Infrastructure
{
    public class TestIDependencyResolver: Test
    {
        [Theory]
        [InlineData(typeof(IDependencyResolver), "TService GetInstance<TService>()")]
        [InlineData(typeof(IDependencyResolver), "Void Register<TService, TImplementation>()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}