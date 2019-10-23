using Kli.Wrappers;

namespace Kli.Core
{
    /// <summary>
    /// Lógica de funcionamento do programa.
    /// </summary>
    public class Engine: IEngine
    {
        /// <summary>
        /// Define as cores padrão no console.
        /// </summary>
        private readonly IConsole _console;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="console">Define as cores padrão no console.</param>
        public Engine(IConsole console)
        {
            _console = console;
        }
        
        public void Run()
        {
            _console.ResetColor();
            
            _console.WriteLine("Hello!!!");
            
            _console.ResetColor();
        }
    }
}