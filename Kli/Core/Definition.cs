using System;
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
        /// Nome do arquivo temporário usado para testar se tem acesso de escrita do disco.
        /// Este atributo existe para fins de testes. Ao definir um valor inválido de arquivo
        /// consegue-se simular falha no acesso de gravação no disco.
        /// </summary>
        public static string TemporaryFilenameForTestIfCanWriteIntoDirectoryOfUser =
            $"_can_delete_{Guid.NewGuid()}.tmp";
            
        /// <summary>
        /// Construtor.
        /// </summary>
        public Definition()
        {
            DirectoryOfUser = new DirectoryInfo(Path.Combine(DirectoryOfProgram, NameOfDirectoryOfUser)).FullName;

            try
            {
                if (!Directory.Exists(DirectoryOfUser)) Directory.CreateDirectory(DirectoryOfUser);
                var temporaryFile = Path.Combine(DirectoryOfUser, TemporaryFilenameForTestIfCanWriteIntoDirectoryOfUser);
                File.WriteAllText(temporaryFile, "");
                File.Delete(temporaryFile);
                CanWriteIntoDirectoryOfUser = true;
            }
            catch
            {
                CanWriteIntoDirectoryOfUser = false;
            }
        }
        
        /// <summary>
        /// Diretório do programa.
        /// </summary>
        public string DirectoryOfProgram { get; } = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName ?? throw new NullReferenceException();

        /// <summary>
        /// Nome do diretório de dados do usuário.
        /// </summary>
        private const string NameOfDirectoryOfUser = "UserData";
        
        /// <summary>
        /// Diretório para dados do usuário.
        /// </summary>
        public string DirectoryOfUser { get; }
        
        /// <summary>
        /// Indica se existe permissão de escrever no diretório do usuário.
        /// </summary>
        public bool CanWriteIntoDirectoryOfUser { get; }
    }
}