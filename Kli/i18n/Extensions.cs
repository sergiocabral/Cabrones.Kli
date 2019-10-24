namespace Kli.i18n
{
    /// <summary>
    /// Funções tipo extensions para internationalization (i18n) 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Retorna a tradução de um texto.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="language">Idioma.</param>
        /// <returns>Tradução.</returns>
        public static string Translate(this string text, string? language = null) =>
            Program.DependencyResolver.GetInstance<ITranslate>().Get(text, language);
    }
}