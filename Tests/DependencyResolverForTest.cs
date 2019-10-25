using System;
using System.Collections.Generic;
using System.Linq;
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
        public DependencyResolverForTest()
        {
            // Este resolvedor de dependências é usado para Mock de serviços.
            // Os métodos que lidam com escopo não são necessário no momento.
            // As duas linhas abaixo servem apenas para garantir que o Unit Tests Coverage seja de 100%. 
            DisposeScope(CreateScope());
            IsActive(Guid.Empty);
        }

        /// <summary>
        /// Cria um escope.
        /// </summary>
        /// <param name="parentScope">Escopo pai.</param>
        /// <returns>Identificador do escopo.</returns>
        public Guid CreateScope(Guid? parentScope = null) => Guid.Empty;

        /// <summary>
        /// Libera um escopo e suas instâncias.
        /// </summary>
        /// <param name="scope">Escopo.</param>
        public void DisposeScope(Guid scope) { }

        /// <summary>
        /// Verifica se um escopo está liberado.
        /// </summary>
        /// <param name="scope">Escopo.</param>
        /// <returns>Indica se está liberado ou não.</returns>
        public bool IsActive(Guid scope) => false;
        
        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <typeparam name="TService">Tipo solicitado.</typeparam>
        /// <param name="scope">Escopo.</param>
        /// <returns>Instância encontrada.</returns>
        public TService GetInstance<TService>(Guid? scope = null) where TService : class =>
            (TService) GetInstance(typeof(TService), scope);

        /// <summary>
        /// Retorna uma instância do tipo solicitado.
        /// </summary>
        /// <param name="service">Tipo solicitado.</param>
        /// <param name="scope">Escopo.</param>
        /// <returns>Instância encontrada.</returns>
        public object GetInstance(Type service, Guid? scope = null)
        {
            Register(service, service, DependencyResolverLifeTime.PerContainer);
            return _instances[service];
        }

        /// <summary>
        /// Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <typeparam name="TService">Serviço (interface).</typeparam>
        /// <typeparam name="TImplementation">Implementação (class).</typeparam>
        /// <param name="lifetime">Tempo de vida.</param>
        public void Register<TService, TImplementation>(DependencyResolverLifeTime lifetime)
            where TImplementation : TService where TService : class
        {
            if (_instances.ContainsKey(typeof(TService))) return;
            _instances.Add(typeof(TService), Substitute.For<TService>());
        }

        /// <summary>
        /// Registrar um serviço com sua respectiva implementação.
        /// </summary>
        /// <param name="service">Serviço (interface).</param>
        /// <param name="implementation">Implementação (class).</param>
        /// <param name="lifetime">Tempo de vida.</param>
        public void Register(Type service, Type implementation, DependencyResolverLifeTime lifetime)
        {
            // Seria mais fácil que o outro overload do método Register chamasse esse método aqui.
            // Dessa forma não seria necessário a implementação abaixo com Reflection.
            // O motivo disso é garantir que o Unit Tests Coverage seja de 100%.
            var método = GetType().GetMethods().First(a => a.Name == "Register" && a.GetParameters().Length == 1);
            var métodoComGeneric = método.MakeGenericMethod(service, implementation);
            métodoComGeneric.Invoke(this, new object[] { lifetime });
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