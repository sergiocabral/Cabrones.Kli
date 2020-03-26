using System.Collections.Generic;

namespace Kli.Core
{
    /// <summary>
    ///     Gerencia múltiplas instâncias da mesma interface.
    /// </summary>
    public interface IMultiple<TService>
    {
        /// <summary>
        ///     Lista de instâncias.
        /// </summary>
        IList<TService> Instances { get; }

        /// <summary>
        ///     Adiciona uma instância na lista.
        /// </summary>
        /// <param name="instance">Instância.</param>
        void Add(TService instance);
    }
}