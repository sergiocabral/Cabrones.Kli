using AutoFixture;

namespace Tests
{
    /// <summary>
    /// Classe base para os testes
    /// </summary>
    public abstract class Test
    {
        /// <summary>
        /// Fixture.
        /// </summary>
        protected Fixture Fixture { get; } = new Fixture();
    }
}