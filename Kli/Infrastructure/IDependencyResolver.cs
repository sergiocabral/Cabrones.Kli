namespace Kli.Infrastructure
{
    /// <summary>
    /// Resolvedor de classes.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <typeparam name="TService">Tipo solicitado.</typeparam>
        /// <returns>Instância encontrada.</returns>
        TService GetInstance<TService>() where TService : class;

        /// <summary>
        /// Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        void Register<TService, TImplementation>() where TImplementation : TService where TService : class;
    }
}