using AutoFixture;
using Kli;
using Kli.Infrastructure;

namespace Tests
{
    /// <summary>
    /// Classe base para os testes
    /// </summary>
    public abstract class Test
    {
        /// <summary>
        /// Construtor. SetUp dos testes.
        /// </summary>
        protected Test()
        {
            Program.DependencyResolver = DependencyResolverFromProgram;
            DependencyResolverForTest.Reset();
        }
        
        /// <summary>
        /// Fixture.
        /// </summary>
        protected Fixture Fixture { get; } = new Fixture();

        /// <summary>
        /// Resolvedor de dependências usado originalmente pelo programa.
        /// </summary>
        protected static IDependencyResolver DependencyResolverFromProgram { get; } = Program.DependencyResolver;

        /// <summary>
        /// Resolvedor de dependências ajustado para atender os testes com Substitute.
        /// </summary>
        protected static DependencyResolverForTest DependencyResolverForTest { get; } = new DependencyResolverForTest();
    }
}