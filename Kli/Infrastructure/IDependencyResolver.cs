using System;
using System.Collections.Generic;

namespace Kli.Infrastructure
{
    /// <summary>
    ///     Resolvedor de classes.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        ///     Lista de interfaces que são implementadas múltiplas vezes.
        ///     Essas interfaces não podem ser registradas como serviço.
        /// </summary>
        IEnumerable<Type> InterfacesForMultipleImplementation { get; }

        /// <summary>
        ///     Cria um escope.
        /// </summary>
        /// <param name="parentScope">Escopo pai.</param>
        /// <returns>Identificador do escopo.</returns>
        Guid CreateScope(Guid? parentScope = null);

        /// <summary>
        ///     Libera um escopo e suas instâncias.
        /// </summary>
        /// <param name="scope">Escopo.</param>
        void DisposeScope(Guid scope);

        /// <summary>
        ///     Verifica se um escopo está liberado.
        /// </summary>
        /// <param name="scope">Escopo.</param>
        /// <returns>Indica se está liberado ou não.</returns>
        bool IsActive(Guid scope);

        /// <summary>
        ///     Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <typeparam name="TService">Tipo solicitado.</typeparam>
        /// <param name="scope">Escopo.</param>
        /// <returns>Instância encontrada.</returns>
        TService GetInstance<TService>(Guid? scope = null) where TService : class;

        /// <summary>
        ///     Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <param name="service">Tipo solicitado.</param>
        /// <param name="scope">Escopo.</param>
        /// <returns>Instância encontrada.</returns>
        object GetInstance(Type service, Guid? scope = null);

        /// <summary>
        ///     Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <typeparam name="TService">Serviço (interface).</typeparam>
        /// <typeparam name="TImplementation">Implementação (class).</typeparam>
        /// <param name="lifetime">Tempo de vida.</param>
        void Register<TService, TImplementation>(
            DependencyResolverLifeTime lifetime = DependencyResolverLifeTime.PerContainer)
            where TImplementation : TService where TService : class;

        /// <summary>
        ///     Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <param name="service">Serviço (interface).</param>
        /// <param name="implementation">Implementação (class).</param>
        /// <param name="lifetime">Tempo de vida.</param>
        void Register(Type service, Type implementation,
            DependencyResolverLifeTime lifetime = DependencyResolverLifeTime.PerContainer);
    }
}