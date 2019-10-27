using Kli.Core;

namespace Kli.Input
{
    /// <summary>
    /// Gerencia múltiplas instâncias da mesma interface: IInput
    /// </summary>
    public interface IMultipleInput: IMultiple<IInput>, IInput
    {
    }
}