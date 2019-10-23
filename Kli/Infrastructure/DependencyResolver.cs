﻿using Kli.Core;
using Kli.i18n;
using Kli.IO;
using Kli.Wrappers;
using LightInject;

namespace Kli.Infrastructure
{
    /// <summary>
    /// Resolvedor de classes.
    /// </summary>
    public class DependencyResolver: IDependencyResolver
    {
        /// <summary>
        /// Construtor.
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
            Register<IEngine, Engine>();
            Register<ICache, Cache>();
            Register<ILanguage, Language>();
            Register<IConsole, Console>();
            Register<IEnvironment, Environment>();
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