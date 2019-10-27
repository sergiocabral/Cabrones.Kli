using Kli.Core;

namespace Kli.Output
{
    /// <summary>
    /// Gerencia múltiplos IOutput.
    /// </summary>
    public class MultipleOutput: Multiple<IOutput>, IMultipleOutput
    {
        /// <summary>
        /// Escreve um texto para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput Write(string text)
        {
            foreach (var instance in Instances)
                instance.Write(text);
            
            return this;
        }

        /// <summary>
        /// Escreve um texto (com quebra de linha) para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput WriteLine(string text)
        {
            foreach (var instance in Instances)
                instance.WriteLine(text);
            
            return this;
        }
    }
}