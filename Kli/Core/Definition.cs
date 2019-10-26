using System.IO;
using System.Reflection;

namespace Kli.Core
{
    /// <summary>
    /// Conjunto de definições para o programa.
    /// </summary>
    public class Definition: IDefinition
    {
        /// <summary>
        /// Diretório do programa.
        /// </summary>
        public string DirectoryOfProgram { get; } = new FileInfo(Assembly.GetExecutingAssembly().Location).FullName;
    }
}