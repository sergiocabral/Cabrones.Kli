using System;

namespace Kli.Core
{
    /// <summary>
    /// Lógica de funcionamento do programa.
    /// </summary>
    public class Engine: IEngine
    {
        /// <summary>
        /// Configurações relacionadas ao console.
        /// </summary>
        private readonly IConsoleConfiguration _consoleConfiguration;

        /// <summary>
        /// Construtor com DependencyResolver
        /// </summary>
        /// <param name="consoleConfiguration">Configurações relacionadas ao console.</param>
        public Engine(IConsoleConfiguration consoleConfiguration)
        {
            _consoleConfiguration = consoleConfiguration;
        }
        
        public void Run()
        {
            _consoleConfiguration.SaveCurrentColor();
            _consoleConfiguration.SetDefaultColor();
            
            Console.WriteLine("Hello!!!");
            
            _consoleConfiguration.RestoreColor();
        }
    }
}