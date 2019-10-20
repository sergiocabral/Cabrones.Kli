using Kli.Core;
using LightInject;

namespace Kli.Infrastructure
{
    /// <summary>
    /// Resolvedor de classes.
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary>
        /// Construtor estático. Configuração inicial.
        /// </summary>
        static DependencyResolver()
        {
            RegisterAssemblies();
        }

        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <typeparam name="TService">Tipo solicitado.</typeparam>
        /// <returns>Instância encontrada.</returns>
        public static TService GetInstance<TService>()
        {
            return Container.GetInstance<TService>();
        }

        /// <summary>
        /// Container de trabalho do LightInject para este projeto.
        /// </summary>
        private static readonly ServiceContainer Container = new ServiceContainer();

        /// <summary>
        /// Registra as interfaces e tipos associados.
        /// </summary>
        private static void RegisterAssemblies()
        {
            Container.Register<IConsoleConfiguration, ConsoleConfiguration>();
            Container.Register<IEngine, Engine>();
        }
    }
}