using System.Reflection;
using Kli.i18n;
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
        /// Manipula traduções de texto.
        /// </summary>
        private readonly ITranslate _translate;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="console">Define as cores padrão no console.</param>
        /// <param name="translate">Manipula traduções de texto.</param>
        public Engine(IConsole console, ITranslate translate)
        {
            _console = console;
            _translate = translate;
        }
        
        public void Run()
        {
            _console.ResetColor();

            LoadTranslates();
            
            _console.WriteLine("Yes".Translate("pt"));
            
            _console.ResetColor();
        }

        /// <summary>
        /// Carregar as traduções padrão do programa.
        /// </summary>
        private void LoadTranslates() =>
            _translate.LoadFromResource(Assembly.GetCallingAssembly(), "Kli.Properties.translates.json");
    }
}