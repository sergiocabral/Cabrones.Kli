namespace Kli.Output.File
{
    /// <summary>
    ///     Envia dados para o usuário via arquivo.
    /// </summary>
    public interface IOutputFile : IOutput
    {
        /// <summary>
        ///     Caminho do arquivo para onde é enviado o texto.
        /// </summary>
        string Path { get; }

        /// <summary>
        ///     Escritor para o usuário no arquivo.
        /// </summary>
        /// <param name="text">Texto.</param>
        void WriteToFile(string text);
    }
}