namespace Kli.IO
{
    /// <summary>
    /// Funções tipo extensions para Input e Output. 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Escapa o texto para escrever no output mesmo os caracteres de marcadores.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <returns>Texto devidamente escapado.</returns>
        public static string EscapeForOutput(this string text) =>
            Program.DependencyResolver.GetInstance<IOutputMarkers>().Escape(text);
    }
}