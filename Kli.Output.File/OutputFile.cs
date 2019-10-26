using System;
using System.IO;
using System.Text.RegularExpressions;
using Kli.Core;

namespace Kli.Output.File
{
    /// <summary>
    /// Envia dados para o usuário via arquivo.
    /// </summary>
    public class OutputFile: IOutputFile
    {
        /// <summary>
        /// Capaz de escrever o texto com a devida formatação.
        /// </summary>
        private readonly IOutputWriter _outputWriter;

        /// <summary>
        /// Conjunto de definições para o programa.
        /// </summary>
        private readonly IDefinition _definition;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="outputWriter">Capaz de escrever o texto com a devida formatação.</param>
        /// <param name="definition">Conjunto de definições para o programa.</param>
        public OutputFile(IOutputWriter outputWriter, IDefinition definition)
        {
            _outputWriter = outputWriter;
            _definition = definition;

            Path = new FileInfo(System.IO.Path.Combine(definition.DirectoryOfUser,
                $"{Regex.Replace(GetType().FullName ?? string.Empty, @"\.\w*$", string.Empty)}.{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log")).FullName;

            if (definition.CanWriteIntoDirectoryOfUser && !System.IO.File.Exists(Path))
                System.IO.File.WriteAllText(Path, string.Empty);
        }
        
        /// <summary>
        /// Escreve um texto para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput Write(string text)
        {
            _outputWriter.Parse(text, WriteToFile);
            return this;
        }

        /// <summary>
        /// Escreve um texto (com quebra de linha) para leitura do usuário.
        /// </summary>
        /// <param name="text">Texto para escrita.</param>
        public IOutput WriteLine(string text) => Write($"{text}\n");
        
        /// <summary>
        /// Caminho do arquivo para onde é enviado o texto.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Escritor para o usuário no arquivo.
        /// </summary>
        /// <param name="text">Texto.</param>
        public void WriteToFile(string text)
        {
            if (!_definition.CanWriteIntoDirectoryOfUser) return;
            System.IO.File.AppendAllText(Path, text);
        }
    }
}