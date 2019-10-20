using Kli.Core;
using Kli.Infrastructure;

namespace Kli
{
    /// <summary>
    /// Classe principal do programa.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Método de entrada para execução do programa.
        /// </summary>
        private static void Main() => DependencyResolver.GetInstance<IEngine>().Run();
    }
}