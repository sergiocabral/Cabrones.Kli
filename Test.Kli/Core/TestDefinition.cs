using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Test;
using Xunit;

namespace Kli.Core
{
    public class TestDefinition: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Definition), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Definition), typeof(IDefinition))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            TestTypeImplementations(tipoDaClasse, tiposQueDeveSerImplementado);

        [Fact]
        public void verificar_se_valor_está_correto_para_propriedade_DirectoryOfProgram()
        {
            // Arrange, Given

            var definition = Program.DependencyResolver.GetInstance<IDefinition>();

            // Act, When

            var valorEsperado = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName;
            var valorAtual = definition.DirectoryOfProgram;

            // Assert, Then

            valorAtual.Should().Be(valorEsperado);
        }
        
        [Fact]
        public void verificar_se_valor_está_correto_para_propriedade_DirectoryOfUser()
        {
            // Arrange, Given

            var definition = Program.DependencyResolver.GetInstance<IDefinition>();

            // Act, When

            var valorEsperado = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName, "UserData");
            var valorAtual = definition.DirectoryOfUser;

            // Assert, Then

            valorAtual.Should().Be(valorEsperado);
        }
        
        [Fact]
        public void verificar_se_a_inicialização_da_propriedade_DirectoryOfUser_cria_o_diretório_caso_não_exista()
        {
            // Arrange, Given

            var diretórioDoUsuario = new DirectoryInfo(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName, "UserData"));
            diretórioDoUsuario.Delete(true);
            
            // Act, When

            var inicialmenteODiretórioNãoExiste = Directory.Exists(diretórioDoUsuario.FullName);
            var _ = new Definition();
            var finalmenteODiretórioExiste = Directory.Exists(diretórioDoUsuario.FullName);

            // Assert, Then

            inicialmenteODiretórioNãoExiste.Should().BeFalse();
            finalmenteODiretórioExiste.Should().BeTrue();
        }
        
        [Fact]
        public void verificar_se_valor_está_correto_para_propriedade_CanWriteIntoDirectoryOfUser()
        {
            // Arrange, Given

            void VerificarCapacidadeDeGravaçãoEmDisco(string arquivo)
            {
                File.WriteAllText(arquivo, "");
                File.Delete(arquivo);
            }
            
            var definitionSemPermissãoDeEscritaNesteArquivo = Definition.TemporaryFilenameForTestIfCanWriteIntoDirectoryOfUser = "***";
            var definitionSemPermissãoDeEscrita = new Definition() as IDefinition;

            var definitionComPermissãoDeEscritaNesteArquivo = Definition.TemporaryFilenameForTestIfCanWriteIntoDirectoryOfUser = $"_pode_apagar_{Guid.NewGuid()}.tmp";
            var definitionComPermissãoDeEscrita = new Definition() as IDefinition;

            // Act, When

            Action definitionSemPermissãoDeEscritaVerificar =
                () => VerificarCapacidadeDeGravaçãoEmDisco(definitionSemPermissãoDeEscritaNesteArquivo);
            Action definitionComPermissãoDeEscritaVerificar =
                () => VerificarCapacidadeDeGravaçãoEmDisco(definitionComPermissãoDeEscritaNesteArquivo);
                
            // Assert, Then

            definitionSemPermissãoDeEscrita.CanWriteIntoDirectoryOfUser.Should().BeFalse();
            definitionSemPermissãoDeEscritaVerificar.Should().Throw<Exception>();

            definitionComPermissãoDeEscrita.CanWriteIntoDirectoryOfUser.Should().BeTrue();
            definitionComPermissãoDeEscritaVerificar.Should().NotThrow();
        } 
    }
}