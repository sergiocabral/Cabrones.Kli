using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Kli.Infrastructure;

namespace Kli.Core
{
    /// <summary>
    /// Carregador de assembly em disco para a memória.
    /// </summary>
    public class LoaderAssembly: ILoaderAssembly
    {
        /// <summary>
        /// Conjunto de definições para o programa.
        /// </summary>
        private readonly IDefinition _definition;

        /// <summary>
        /// Resolvedor de classes.
        /// </summary>
        private readonly IDependencyResolver _dependencyResolver;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="definition">Conjunto de definições para o programa.</param>
        /// <param name="dependencyResolver">Resolvedor de classes.</param>
        public LoaderAssembly(IDefinition definition, IDependencyResolver dependencyResolver)
        {
            _definition = definition;
            _dependencyResolver = dependencyResolver;
        }
        
        /// <summary>
        /// Carrega um ou mais arquivos de assembly.
        /// </summary>
        /// <param name="fileMask">Máscara de busca dos arquivos.</param>
        /// <returns>Lista de arquivos com seu respectivo assembly. Quando assembly é null indica falha no carregamento.</returns>
        public IDictionary<string, Assembly?> Load(string fileMask)
        {
            var result = new Dictionary<string, Assembly?>();
            
            FileInfo[] files;
            try
            {
                files = new DirectoryInfo(_definition.DirectoryOfProgram).GetFiles(fileMask);
            }
            catch
            {
                return result;
            }
            
            foreach (var file in files)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    assembly.GetTypes();
                    result.Add(file.FullName, assembly);
                }
                catch
                {
                    result.Add(file.FullName, null);
                }
            }
            return result;
        }

        /// <summary>
        /// Carrega um ou mais arquivos de assembly e registra as instâncias no resolvedor de dependências.
        /// </summary>
        /// <param name="fileMask">Máscara de busca dos arquivos.</param>
        /// <returns>Lista de serviços (tipos) registrados.</returns>
        public IEnumerable<Type> RegisterServices(string fileMask)
        {
            var interfaces = new List<Type>();

            foreach (var assembly in Load(fileMask).Select(a => a.Value))
            {
                if (assembly == null) continue;
                foreach (var type in assembly.GetTypes().Where(a => a.IsClass && !a.IsAbstract && a.GetInterfaces().Length > 0))
                {
                    foreach (var interfaceType in type.GetInterfaces())
                    {
                        _dependencyResolver.Register(interfaceType, type, DependencyResolverLifeTime.PerScope);
                        interfaces.Add(interfaceType);
                    }
                }
            }

            return interfaces.ToArray();
        }

        /// <summary>
        /// Carrega serviços de um determinado tipo presentes em um ou mais arquivos de assembly.
        /// </summary>
        /// <param name="fileMask">Máscara de busca dos arquivos.</param>
        /// <typeparam name="TService">Serviço.</typeparam>
        /// <returns>Lista das instâncias criadas para o serviço informado.</returns>
        public IEnumerable<TService> GetInstances<TService>(string fileMask) where TService : class
        {
            var scope = _dependencyResolver.CreateScope();
            return new List<TService>(
                from type in RegisterServices(fileMask) 
                where typeof(TService) == type 
                select (TService) _dependencyResolver.GetInstance(type, scope)
            ).ToArray();
        }
    }
}