using System.Linq;
using System.Reflection;
using System.Threading;
using Kli.Core;

namespace Kli.Input
{
    /// <summary>
    ///     Gerencia múltiplos IInput.
    /// </summary>
    public class MultipleInput : Multiple<IInput>, IMultipleInput
    {
        /// <summary>
        ///     Tempo de espera quando algum loop está girando.
        /// </summary>
        private const int SleepWhenInLoop = 100;

        /// <summary>
        ///     Faz a leitura de um texto da parte do usuário, mas espera concluir com Enter.
        ///     Avança linha após o Enter.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        public string ReadLine(bool isSensitive = false)
        {
            return CallReadMethod("ReadLine", isSensitive);
        }

        /// <summary>
        ///     Faz a leitura de um texto da parte do usuário, mas espera concluir com Enter.
        ///     Não avança linha após o Enter e apaga o texto digitado.
        /// </summary>
        /// <param name="isSensitive">Indica se o dado é sensível.</param>
        /// <returns>Valor do usuário.</returns>
        public string Read(bool isSensitive = false)
        {
            return CallReadMethod("Read", isSensitive);
        }

        /// <summary>
        ///     Faz a leitura de um texto da parte do usuário, mas conclui imediatamente na primeira tecla.
        /// </summary>
        /// <returns>Caracter recebido.</returns>
        public string ReadKey()
        {
            return CallReadMethod("ReadKey");
        }

        /// <summary>
        ///     Verifica se possui resposta disponível.
        /// </summary>
        public bool HasRead()
        {
            return Instances.Any(a => a.HasRead());
        }

        /// <summary>
        ///     Faz a leitura de um texto da parte do usuário.
        /// </summary>
        /// <param name="method">Nome do método que será chamado.</param>
        /// <param name="parameters">Parâmetros para o método.</param>
        /// <returns>Retorno.</returns>
        private string CallReadMethod(string method, params object[] parameters)
        {
            string? result = null;

            var methodInfo = typeof(IInput).GetMethod(method, BindingFlags.Instance | BindingFlags.Public);
            do
            {
                foreach (var instance in Instances)
                {
                    if (!instance.HasRead()) continue;

                    result = methodInfo?.Invoke(instance, parameters) as string;
                    break;
                }

                Thread.Sleep(SleepWhenInLoop);
            } while (result == null);

            return result;
        }
    }
}