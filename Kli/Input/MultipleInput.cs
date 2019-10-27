using System.Linq;
using System.Threading;
using Kli.Core;

namespace Kli.Input
{
    /// <summary>
    /// Gerencia múltiplos IInput.
    /// </summary>
    public class MultipleInput: Multiple<IInput>, IMultipleInput
    {
        /// <summary>
        /// Tempo de espera quando algum loop está girando.
        /// </summary>
        private const int SleepWhenInLoop = 100;
        
        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas espera concluir com Enter.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        public string Read(bool isSensitive = false)
        {
            do
            {
                foreach (var instance in Instances)
                {
                    if (instance.HasRead()) return instance.Read(isSensitive);
                }
                Thread.Sleep(SleepWhenInLoop);
            } while (true);
        }

        /// <summary>
        /// Faz a leitura de um texto da parte do usuário, mas conclui imediatamente na primeira tecla.
        /// </summary>
        /// <returns>Caracter recebido.</returns>
        public string ReadKey()
        {
            do
            {
                foreach (var instance in Instances)
                {
                    if (instance.HasRead()) return instance.ReadKey();
                }
                Thread.Sleep(SleepWhenInLoop);
            } while (true);
        }

        /// <summary>
        /// Verifica se possui resposta disponível.
        /// </summary>
        public bool HasRead() => Instances.Any(a => a.HasRead());
    }
}