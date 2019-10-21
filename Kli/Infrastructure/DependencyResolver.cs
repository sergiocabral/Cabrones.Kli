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
            _container.Register<IOutputWriter, OutputWriter>(new PerContainerLifetime());
            _container.Register<IOutputMarkers, OutputMarkers>(new PerContainerLifetime());
            _container.Register<IConsoleConfiguration, ConsoleConfiguration>(new PerContainerLifetime());
            _container.Register<IEngine, Engine>(new PerContainerLifetime());
            _container.Register<IDependencyResolver, DependencyResolver>(new PerContainerLifetime());
        }
    }
}