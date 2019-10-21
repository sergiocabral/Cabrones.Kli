using Kli.Common.IO;
using Kli.Core;
using LightInject;

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
    }
    
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
            _container.Register<IOutputMarkers, OutputMarkers>();
            _container.Register<IConsoleConfiguration, ConsoleConfiguration>();
            _container.Register<IEngine, Engine>();
            _container.Register<IDependencyResolver, DependencyResolver>();
        }
    }
}