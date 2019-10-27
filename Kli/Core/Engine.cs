using System.Reflection;
using Kli.i18n;
using Kli.Input;
using Kli.Module;
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
        /// Gerencia múltiplos IInput.
        /// </summary>
        private readonly IMultipleInput _multipleInput;
        
        /// <summary>
        /// Gerencia múltiplos IOutput.
        /// </summary>
        private readonly IMultipleOutput _multipleOutput;
        
        /// <summary>
        /// Gerencia múltiplos IModule.
        /// </summary>
        private readonly IMultipleModule _multipleModule;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="console">Define as cores padrão no console.</param>
        /// <param name="translate">Manipula traduções de texto.</param>
        /// <param name="loaderAssembly">Carregador de assembly em disco para a memória.</param>
        /// <param name="multipleInput">Gerencia múltiplos IInput.</param>
        /// <param name="multipleOutput">Gerencia múltiplos IOutput.</param>
        /// <param name="multipleModule">Gerencia múltiplos IModule.</param>
        public Engine(
            IConsole console, 
            ITranslate translate, 
            ILoaderAssembly loaderAssembly,
            IMultipleInput multipleInput,
            IMultipleOutput multipleOutput,
            IMultipleModule multipleModule)
        {
            _console = console;
            _translate = translate;
            _loaderAssembly = loaderAssembly;
            _multipleInput = multipleInput;
            _multipleOutput = multipleOutput;
            _multipleModule = multipleModule;
        }
        
        /// <summary>
        /// Preparação inicial para então executar os módulos.
        /// </summary>
        public void Initialize()
        {
            ResetConsole();
            LoadTranslates();
            LoadAssemblies();
            RunModules();
            ResetConsole();
        }
        
        /// <summary>
        /// Redefine as cores do console. 
        /// </summary>
        private void ResetConsole() => _console.ResetColor();

        /// <summary>
        /// Carregar as traduções padrão do programa.
        /// </summary>
        private void LoadTranslates() =>
            _translate.LoadFromResource(Assembly.GetCallingAssembly(), "Kli.Properties.translates.json");

        /// <summary>
        /// Carrega os assemblies em tempo de execução.
        /// </summary>
        private void LoadAssemblies()
        {
            LoadAssemblies(_multipleOutput,"Kli.Output.*.dll");
            LoadAssemblies(_multipleInput,"Kli.Input.*.dll");
            LoadAssemblies(_multipleModule,"Kli.Module.*.dll");
        }

        /// <summary>
        /// Carrega os assemblies em tempo de execução.
        /// </summary>
        /// <param name="multiple">Gerenciador de múltiplas interfaces.</param>
        /// <param name="fileMask">Máscara de busca dos arquivos.</param>
        /// <typeparam name="TService">Tipo do serviço a ser carregado.</typeparam>
        private void LoadAssemblies<TService>(IMultiple<TService> multiple, string fileMask) where TService : class
        {
            foreach (var instance in _loaderAssembly.GetInstances<TService>(fileMask))
                multiple.Add(instance);
        }

        /// <summary>
        /// Inicia a execução dos módulos carregados. 
        /// </summary>
        private void RunModules() => _multipleModule.Run();
    }
}