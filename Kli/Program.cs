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
        /// Resolvedor de dependências padrão para o programa.
        /// </summary>
        public static IDependencyResolver DependencyResolver { get; set; } = new DependencyResolver();
        
        /// <summary>
        /// Método de entrada para execução do programa.
        /// </summary>
        public static void Main() => DependencyResolver.GetInstance<IEngine>().Run();
    }
}