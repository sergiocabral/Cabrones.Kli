using Kli.Core;

namespace Kli.Output
{
    /// <summary>
    /// Gerencia múltiplas instâncias da mesma interface: IOutput
    /// </summary>
    public interface IMultipleOutput: IMultiple<IOutput>, IOutput
    {
    }
}