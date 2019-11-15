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
        /// Resolvedor de dependência padrão.
        /// </summary>
        private static readonly IDependencyResolver DependencyResolverDefault = new DependencyResolver();
        
        /// <summary>
        /// Resolvedor de dependência para os métodos desta classe.
        /// </summary>
        private static IDependencyResolver? _dependencyResolver;

        /// <summary>
        /// Resolvedor de dependência para os métodos desta classe.
        /// Quando definido como nulo, usa o padrão em Program.DependencyResolver.
        /// </summary>
        public static IDependencyResolver DependencyResolver
        {
            get => _dependencyResolver ?? DependencyResolverDefault;
            set => _dependencyResolver = value;
        }
        
        /// <summary>
        /// Método de entrada para execução do programa.
        /// </summary>
        public static void Main() => DependencyResolver.GetInstance<IEngine>().Initialize();
    }
}