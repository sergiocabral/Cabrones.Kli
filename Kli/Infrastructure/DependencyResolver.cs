using System;
using System.Collections.Generic;
using System.Linq;
using Kli.Core;
using Kli.i18n;
using Kli.Input;
using Kli.Output;
using Kli.Wrappers;
using LightInject;
using Console = Kli.Wrappers.Console;
using Environment = Kli.Wrappers.Environment;

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
        /// Cria um escope.
        /// </summary>
        /// <param name="parentScope">Escopo pai.</param>
        /// <returns>Identificador do escopo.</returns>
        public Guid CreateScope(Guid? parentScope = null)
        {
            ValidateScope(parentScope);
            var id = Guid.NewGuid();
            var scope = parentScope.HasValue ? Scopes[parentScope.Value].BeginScope() : Container.BeginScope();
            Scopes.Add(id, scope);
            scope.Completed += (sender, args) =>
            {
                foreach (var (childId, _) in Scopes.Where(a => a.Value.ParentScope == Scopes[id]))
                    DisposeScope(childId);
                Scopes.Remove(id);
            }; 
            return id;
        }

        /// <summary>
        /// Libera um escopo e suas instâncias.
        /// </summary>
        /// <param name="scope">Escopo.</param>
        public void DisposeScope(Guid scope)
        {
            ValidateScope(scope);
            Scopes[scope].Dispose();
        }

        /// <summary>
        /// Verifica se um escopo está liberado.
        /// </summary>
        /// <param name="scope">Escopo.</param>
        /// <returns>Indica se está liberado ou não.</returns>
        public bool IsActive(Guid scope)
        {
            return Scopes.ContainsKey(scope);
        }

        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <typeparam name="TService">Tipo solicitado.</typeparam>
        /// <param name="scope">Escopo.</param>
        /// <returns>Instância encontrada.</returns>
        public TService GetInstance<TService>(Guid? scope) where TService : class =>
            (TService) GetInstance(typeof(TService), scope);

        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <param name="service">Tipo solicitado.</param>
        /// <param name="scope">Escopo.</param>
        /// <returns>Instância encontrada.</returns>
        public object GetInstance(Type service, Guid? scope)
        {
            if (!scope.HasValue) return Container.GetInstance(service);
            ValidateScope(scope.Value);
            return Scopes[scope.Value].GetInstance(service);
        }

        /// <summary>
        /// Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <typeparam name="TService">Serviço (interface).</typeparam>
        /// <typeparam name="TImplementation">Implementação (class).</typeparam>
        /// <param name="lifetime">Tempo de vida.</param>
        public void Register<TService, TImplementation>(DependencyResolverLifeTime lifetime) 
            where TImplementation : TService where TService : class =>
            Container.Register<TService, TImplementation>(GetILifeTime(lifetime));

        /// <summary>
        /// Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <param name="service">Serviço (interface).</param>
        /// <param name="implementation">Implementação (class).</param>
        /// <param name="lifetime">Tempo de vida.</param>
        public void Register(Type service, Type implementation, DependencyResolverLifeTime lifetime)
        {
            if (InterfacesForMultipleImplementation.Contains(service)) throw new ArgumentException();
            Container.Register(service, implementation, GetILifeTime(lifetime));
        }

        /// <summary>
        /// Lista de interfaces que são implementadas múltiplas vezes.
        /// Essas interfaces não podem ser registradas como serviço.
        /// </summary>
        public IEnumerable<Type> InterfacesForMultipleImplementation { get; } = new[] {typeof(IOutput), typeof(IInput)};

        /// <summary>
        /// Container de trabalho do LightInject para este projeto.
        /// </summary>
        private static readonly ServiceContainer Container = new ServiceContainer();
        
        /// <summary>
        /// Lista de escopos abertos.
        /// </summary>
        private static readonly IDictionary<Guid, Scope> Scopes = new Dictionary<Guid, Scope>();

        /// <summary>
        /// Registra as interfaces e tipos associados.
        /// </summary>
        private void RegisterAssemblies()
        {
            Register<IOutputWriter, OutputWriter>(DependencyResolverLifeTime.PerContainer);
            Register<IOutputMarkers, OutputMarkers>(DependencyResolverLifeTime.PerContainer);
            Register<IEngine, Engine>(DependencyResolverLifeTime.PerContainer);
            Register<ICache, Cache>(DependencyResolverLifeTime.PerContainer);
            Register<ILanguage, Language>(DependencyResolverLifeTime.PerContainer);
            Register<ITranslate, Translate>(DependencyResolverLifeTime.PerContainer);
            Register<ILoaderAssembly, LoaderAssembly>(DependencyResolverLifeTime.PerContainer);
            Register<IConsole, Console>(DependencyResolverLifeTime.PerContainer);
            Register<IEnvironment, Environment>(DependencyResolverLifeTime.PerContainer);
            Register<IDefinition, Definition>(DependencyResolverLifeTime.PerContainer);
            Register<IDependencyResolver, DependencyResolver>(DependencyResolverLifeTime.PerContainer);
        }

        /// <summary>
        /// Converte para o ILifetime correspondente.
        /// </summary>
        /// <param name="lifetime">Enum</param>
        /// <returns>ILifetime</returns>
        private static ILifetime GetILifeTime(DependencyResolverLifeTime lifetime)
        {
            if (lifetime == DependencyResolverLifeTime.PerScope) return new PerScopeLifetime();
            return new PerContainerLifetime();
        }

        /// <summary>
        /// Verifica de um escopo é válido.
        /// </summary>
        /// <param name="scope">Escopo.</param>
        /// <exception cref="ObjectDisposedException">Quando não é válido.</exception>
        private void ValidateScope(Guid? scope)
        {
            if (!scope.HasValue) return;
            if (!IsActive(scope.Value)) throw new ObjectDisposedException(scope.Value.ToString());
        } 
    }
}