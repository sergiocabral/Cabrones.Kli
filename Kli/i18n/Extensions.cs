using Kli.Infrastructure;

namespace Kli.i18n
{
    /// <summary>
    /// Funções tipo extensions para internationalization (i18n) 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Resolvedor de dependência para os métodos desta classe.
        /// </summary>
        private static IDependencyResolver _dependencyResolver;
        
        /// <summary>
        /// Resolvedor de dependência para os métodos desta classe.
        /// Quando definido como nulo, usa o padrão em Program.DependencyResolver.
        /// </summary>
        public static IDependencyResolver DependencyResolver
        {
            get => _dependencyResolver ?? Program.DependencyResolver;
            set => _dependencyResolver = value;
        }
        
        /// <summary>
        /// Retorna a tradução de um texto.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="language">Idioma.</param>
        /// <returns>Tradução.</returns>
        public static string Translate(this string text, string? language = null) =>
            DependencyResolver.GetInstance<ITranslate>().Get(text, language);
    }
}