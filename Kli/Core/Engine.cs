using System;

namespace Kli.Core
{
    /// <summary>
    /// Lógica de funcionamento do programa.
    /// </summary>
    public class Engine: IEngine
    {
        public void Run()
        {
            Console.ResetColor();
            
            Console.WriteLine("Hello!!!");
            
            Console.ResetColor();
        }
    }
}