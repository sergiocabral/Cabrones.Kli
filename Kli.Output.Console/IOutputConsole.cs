namespace Kli.Output.Console
{
    /// <summary>
    ///     Envia dados para o usuário via console.
    /// </summary>
    public interface IOutputConsole : IOutput
    {
        /// <summary>
        ///     Escritor para o usuário no console.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="marker">Marcador aplicado ao texto.</param>
        void WriteToConsole(string text, char marker = (char) 0);

        /// <summary>
        ///     Retorna o marcador com base nas cores atuais do console.
        /// </summary>
        /// <returns>Marcador.</returns>
        char CurrentMarker();
    }
}