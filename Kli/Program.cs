using Kli.Core;
using Kli.Infrastructure;

namespace Kli
{
    /// <summary>
    /// Classe principal do programa.
    /// </summary>
    public static class Program
    {   
        /// <summary>
        /// Método de entrada para execução do programa.
        /// </summary>
        public static void Main() => DependencyResolver.Default.GetInstance<IEngine>().Run();
    }
}