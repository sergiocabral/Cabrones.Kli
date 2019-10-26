namespace Kli.Input
{
    /// <summary>
    /// Interface para classe que recebe entrada de dados do usuário.  
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Faz a leitura de um texto da parte  do usuário.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        string Read(bool isSensitive = false);
    }
}