namespace Kli.Infrastructure
{
    /// <summary>
    ///     Tempo de vida possíveis para um serviço/implementação do IDependencyResolver
    /// </summary>
    public enum DependencyResolverLifeTime
    {
        /// <summary>
        ///     Único por execução do programa.
        /// </summary>
        PerContainer,

        /// <summary>
        ///     Único em um dado escopo.
        /// </summary>
        PerScope
    }
}