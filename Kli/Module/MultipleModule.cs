using Kli.Core;

namespace Kli.Module
{
    /// <summary>
    /// Gerencia múltiplos IModule.
    /// </summary>
    public class MultipleModule: Multiple<IModule>, IMultipleModule
    {
        /// <summary>
        /// Agrupa funções utilitárias para lidar com a interação com o usuário.
        /// </summary>
        private readonly IInteraction _interaction;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="interaction">Agrupa funções utilitárias para lidar com a interação com o usuário.</param>
        public MultipleModule(IInteraction interaction)
        {
            _interaction = interaction;
        }

        /// <summary>
        /// Método principal do módulo. Inicia sua execução.
        /// </summary>
        public void Run() => _interaction.StartInteraction();
    }
}