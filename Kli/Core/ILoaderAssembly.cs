using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kli.Core
{
    /// <summary>
    /// Carregador de assembly em disco para a memória.
    /// </summary>
    public interface ILoaderAssembly
    {


        /// <summary>
        /// Carrega um ou mais arquivos de assembly.
        /// </summary>
        /// <param name="fileMask">Máscara de busca dos arquivos.</param>
        /// <returns>Lista de arquivos com seu respectivo assembly. Quando assembly é null indica falha no carregamento.</returns>
        IDictionary<string, Assembly?> Load(string fileMask);

        /// <summary>
        /// Carrega um ou mais arquivos de assembly e registra as instâncias no resolvedor de dependências.
        /// </summary>
        /// <param name="fileMask">Máscara de busca dos arquivos.</param>
        /// <returns>Lista de serviços (tipos) registrados.</returns>
        IEnumerable<Type> RegisterServices(string fileMask);

        /// <summary>
        /// Carrega serviços de um determinado tipo presentes em um ou mais arquivos de assembly.
        /// </summary>
        /// <param name="fileMask">Máscara de busca dos arquivos.</param>
        /// <typeparam name="TService">Serviço.</typeparam>
        /// <returns>Lista das instâncias criadas para o serviço informado.</returns>
        IEnumerable<TService> GetInstances<TService>(string fileMask) where TService : class;
    }
}