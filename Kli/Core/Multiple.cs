using System.Collections.Generic;
using System.Linq;

namespace Kli.Core
{
    /// <summary>
    ///     Gerencia múltiplas instâncias da mesma interface.
    /// </summary>
    public abstract class Multiple<TService> : IMultiple<TService>
    {
        /// <summary>
        ///     Lista original para: Instances.
        /// </summary>
        private readonly IList<TService> _instances = new List<TService>();

        /// <summary>
        ///     Lista de instâncias.
        /// </summary>
        public IList<TService> Instances => _instances.ToList();

        /// <summary>
        ///     Adiciona uma instância na lista.
        /// </summary>
        /// <param name="instance">Instância.</param>
        public void Add(TService instance)
        {
            if (_instances.Contains(instance)) return;
            _instances.Add(instance);
        }
    }
}