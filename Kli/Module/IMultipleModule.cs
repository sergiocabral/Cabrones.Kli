using Kli.Core;

namespace Kli.Module
{
    /// <summary>
    /// Gerencia múltiplas instâncias da mesma interface: IModule
    /// </summary>
    public interface IMultipleModule: IMultiple<IModule>, IModule
    {
    }
}