using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using Cabrones.Test;
using Xunit;

namespace Kli.Core
{
    public class TestDefinition
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Definition);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IDefinition));
            sut.AssertMyOwnImplementations(
                typeof(IDefinition));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
        
        [Fact]
        public void o_arquivo_temporário_de_teste_deve_ser_válido()
        {
            // Arrange, Given

            var nomeDoArquivo = Definition.TemporaryFilenameForTestIfCanWriteIntoDirectoryOfUser;

            const string máscaraRegex = @"_can_delete_[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}\.tmp";
            var testeDaMáscara = this.Fixture<string>();

            // Act, When

            var fileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, nomeDoArquivo));

            // Assert, Then

            fileInfo.Name.Should().Be(nomeDoArquivo);
            Regex.Replace(fileInfo.Name, máscaraRegex, testeDaMáscara).Should().Be(testeDaMáscara);
        }
        
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