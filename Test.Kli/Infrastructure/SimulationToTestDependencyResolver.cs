using System;

namespace Kli.Infrastructure
{
    /// <summary>
    ///     Classe usada como insumo para testar o DependencyResolver.
    /// </summary>
    internal class SimulationToTestDependencyResolver : IDisposable
    {
        /// <summary>
        ///     Construtor. Define o identificador.
        /// </summary>
        public SimulationToTestDependencyResolver()
        {
            Identificador = DateTime.Now.Ticks;
        }

        /// <summary>
        ///     Identificador da instância.
        /// </summary>
        public long Identificador { get; }

        /// <summary>
        ///     Dispose.
        /// </summary>
        public void Dispose()
        {
            Disposed?.Invoke();
        }

        /// <summary>
        ///     Evento disparado quando Dispose é chamado.
        /// </summary>
        public event Action Disposed;
    }
}