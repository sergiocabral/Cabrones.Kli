using Kli.Infrastructure;

namespace Kli.Output
{
    /// <summary>
    ///     Funções tipo extensions para Output.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Resolvedor de dependência para os métodos desta classe.
        /// </summary>
        private static IDependencyResolver? _dependencyResolver;

        /// <summary>
        ///     Resolvedor de dependência para os métodos desta classe.
        ///     Quando definido como nulo, usa o padrão em Program.DependencyResolver.
        /// </summary>
        public static IDependencyResolver DependencyResolver
        {
            get => _dependencyResolver ?? Program.DependencyResolver;
            set => _dependencyResolver = value;
        }

        /// <summary>
        ///     Escapa o texto para escrever no output mesmo os caracteres de marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto devidamente escapado.</returns>
        public static string EscapeForOutput(this string text)
        {
            return DependencyResolver.GetInstance<IOutputMarkers>().Escape(text);
        }
    }
}