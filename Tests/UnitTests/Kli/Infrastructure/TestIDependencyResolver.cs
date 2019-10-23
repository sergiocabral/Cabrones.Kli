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
        public void verifica_se_assinatura_de_métodos_existe(Type tipo, string assinaturaEsperada)
        {
            verifica_se_assinatura_de_método_existe(tipo, assinaturaEsperada);
        }

    }
}