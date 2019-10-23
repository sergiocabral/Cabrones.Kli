using Kli.Core;
using Kli.IO;
using LightInject;

namespace Kli.Infrastructure
{
    /// <summary>
    /// Resolvedor de classes.
    /// </summary>
    public class DependencyResolver: IDependencyResolver
    {
        /// <summary>
        /// Construtor estático. Configuração inicial.
        /// </summary>
        public DependencyResolver()
        {
            RegisterAssemblies();
        }

        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <typeparam name="TService">Tipo solicitado.</typeparam>
        /// <returns>Instância encontrada.</returns>
        public TService GetInstance<TService>() where TService : class
        {
            return _container.GetInstance<TService>();
        }

        /// <summary>
        /// Container de trabalho do LightInject para este projeto.
        /// </summary>
        private readonly ServiceContainer _container = new ServiceContainer();

        /// <summary>
        /// Registra as interfaces e tipos associados.
        /// </summary>
        private void RegisterAssemblies()
        {
            Register<IOutputWriter, OutputWriter>();
            Register<IOutputMarkers, OutputMarkers>();
            Register<ICache, Cache>();
            Register<IEngine, Engine>();
            Register<IDependencyResolver, DependencyResolver>();
        }

        /// <summary>
        /// Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <typeparam name="TService">Serviço.</typeparam>
        /// <typeparam name="TImplementation">Implementação.</typeparam>
        public void Register<TService, TImplementation>() where TImplementation : TService where TService : class
        {
            _container.Register<TService, TImplementation>(new PerContainerLifetime());
        }
    }
}