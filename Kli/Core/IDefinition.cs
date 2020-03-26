namespace Kli.Core
{
    /// <summary>
    ///     Conjunto de definições para o programa.
    /// </summary>
    public interface IDefinition
    {
        /// <summary>
        ///     Diretório do programa.
        /// </summary>
        string DirectoryOfProgram { get; }

        /// <summary>
        ///     Diretório para dados do usuário.
        /// </summary>
        string DirectoryOfUser { get; }

        /// <summary>
        ///     Indica se existe permissão de escrever no diretório do usuário.
        /// </summary>
        bool CanWriteIntoDirectoryOfUser { get; }
    }
}