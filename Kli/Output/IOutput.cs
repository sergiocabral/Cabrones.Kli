namespace Kli.Output
{
    /// <summary>
    /// Interface para classe que envia dados para o usuário.  
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Escreve um texto para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        IOutput Write(string text);
        
        /// <summary>
        /// Escreve um texto (com quebra de linha) para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        IOutput WriteLine(string text);
    }
}