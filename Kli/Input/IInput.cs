namespace Kli.Input
{
    /// <summary>
    /// Interface para classe que recebe entrada de dados do usuário.  
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas espera concluir com Enter.
        /// Avança linha após o Enter e mantém o texto digitado.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        string ReadLine(bool isSensitive = false);
        
        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas espera concluir com Enter.
        /// Não avança linha após o Enter e apaga o texto digitado.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        string Read(bool isSensitive = false);

        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas conclui imediatamente na primeira tecla.
        /// </summary>
        /// <returns>Caracter recebido.</returns>
        string ReadKey();

        /// <summary>
        /// Verifica se possui resposta disponível.
        /// </summary>
        bool HasRead();
    }
}