using System.Reflection;
using Kli.i18n;
using Kli.Input;
using Kli.Output;
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
        /// Carregador de assembly em disco para a memória.
        /// </summary>
        private readonly ILoaderAssembly _loaderAssembly;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="console">Define as cores padrão no console.</param>
        /// <param name="translate">Manipula traduções de texto.</param>
        /// <param name="loaderAssembly">Carregador de assembly em disco para a memória.</param>
        public Engine(IConsole console, ITranslate translate, ILoaderAssembly loaderAssembly)
        {
            _console = console;
            _translate = translate;
            _loaderAssembly = loaderAssembly;
        }
        
        public void Run()
        {
            _console.ResetColor();

            LoadTranslates();
            LoadAssemblies();
            
            _console.WriteLine("Yes".Translate("pt"));
            
            _console.ResetColor();
        }

        /// <summary>
        /// Carrega os assemblies em tempo de execução.
        /// </summary>
        private void LoadAssemblies()
        {
            _loaderAssembly.GetInstances<IOutput>("Kli.Output.*.dll");
            _loaderAssembly.GetInstances<IInput>("Kli.Input.*.dll");
        }

        /// <summary>
        /// Carregar as traduções padrão do programa.
        /// </summary>
        private void LoadTranslates() =>
            _translate.LoadFromResource(Assembly.GetCallingAssembly(), "Kli.Properties.translates.json");
    }
}