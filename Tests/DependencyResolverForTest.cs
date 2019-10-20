using System;
using System.Collections.Generic;
using Kli.Infrastructure;
using NSubstitute;

namespace Tests
{
    /// <summary>
    /// Resolver de dependências para realizar testes.
    /// Cada serviço retornado será do tipo Substitute.
    /// </summary>
    public class DependencyResolverForTest: IDependencyResolver
    {
        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <typeparam name="TService">Tipo solicitado.</typeparam>
        /// <returns>Instância encontrada.</returns>
        public TService GetInstance<TService>() where TService : class
        {
            if (!_instances.ContainsKey(typeof(TService)))
            {
                _instances.Add(typeof(TService), Substitute.For<TService>());
            }
            return (TService)_instances[typeof(TService)];
        }

        /// <summary>
        /// Descarta as instâncias já criadas.
        /// Causa um reset na contagem do Substitute.
        /// </summary>
        public void Reset()
        {
            _instances.Clear();
        }
        
        /// <summary>
        /// Histórico de instâncias utilizadas.
        /// </summary>
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();
    }
}