using System;
using System.IO;
using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using NSubstitute;
using Cabrones.Test;
using Xunit;

namespace Kli.Output.File
{
    public class TestOutputFile
    {
        public TestOutputFile()
        {
            Program.DependencyResolver.Register<IOutputFile, OutputFile>();
        }
        
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(OutputFile);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IOutput), 
                typeof(IOutputFile));
            sut.AssertMyOwnImplementations(
                typeof(IOutputFile));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
        
        [Fact]
        public void verifica_se_o_nome_do_arquivo_está_correto()
        {
            // Arrange, Given

            var definition = Program.DependencyResolver.GetInstance<IDefinition>();
            var caminhoEsperado = Path.Combine(definition.DirectoryOfUser,
                $"{Regex.Replace(typeof(OutputFile).FullName ?? throw new NullReferenceException(), @"\.\w*$", string.Empty)}.{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log");

            // Act, When

            var outputFile = new OutputFile(Substitute.For<IOutputWriter>(), definition) as IOutputFile;
            
            // Assert, Then

            outputFile.Path.Should().Be(caminhoEsperado);
        }
        
        [Fact]
        public void não_pode_criar_o_arquivo_caso_já_exista()
        {
            // Arrange, Given

            var definition = Program.DependencyResolver.GetInstance<IDefinition>();
            var caminhoDoArquivo = Path.Combine(definition.DirectoryOfUser,
                $"{Regex.Replace(typeof(OutputFile).FullName ?? throw new NullReferenceException(), @"\.\w*$", string.Empty)}.{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log");

            var conteudoDoArquivoAoEscrever = this.Fixture<string>();
            System.IO.File.WriteAllText(caminhoDoArquivo, conteudoDoArquivoAoEscrever);

            // Act, When

            var _ = new OutputFile(Substitute.For<IOutputWriter>(), definition);
            var conteudoDoArquivoLido = System.IO.File.ReadAllText(caminhoDoArquivo);
                
            // Assert, Then

            conteudoDoArquivoLido.Should().Be(conteudoDoArquivoAoEscrever);
        }
        
        [Fact]
        public void ao_escrever_um_texto_o_método_WriteToFile_deve_ser_chamado()
        {
            // Arrange, Given

            var outputWriter = Substitute.For<IOutputWriter>();
            var outputFile = new OutputFile(outputWriter, Program.DependencyResolver.GetInstance<IDefinition>()) as IOutputFile;
            var textoDeExemplo = this.Fixture<string>();
            
            // Act, When

            outputFile.Write(textoDeExemplo);
            outputFile.WriteLine(textoDeExemplo);
            
            // Assert, Then

            outputWriter.Received(1).Parse(textoDeExemplo, outputFile.WriteToFile);
            outputWriter.Received(1).Parse($"{textoDeExemplo}\n", outputFile.WriteToFile);
        }

        [Fact]
        public void o_método_WriteToFile_deve_usar_escrever_de_fato_no_arquivo()
        {
            // Arrange, Given

            var outputFile = new OutputFile(Substitute.For<IOutputWriter>(), 
                Program.DependencyResolver.GetInstance<IDefinition>()) as IOutputFile;
            var textoEscrito = this.Fixture<string>();
            
            System.IO.File.WriteAllText(outputFile.Path, string.Empty);
            
            // Act, When
            
            outputFile.WriteToFile(textoEscrito);
            
            // Assert, Then

            System.IO.File.ReadAllText(outputFile.Path).Should().Be(textoEscrito);
        }

        [Fact]
        public void o_método_WriteToFile_só_pode_escrever_no_arquivo_se_tiver_permissão_para_isso()
        {
            // Arrange, Given

            var definition = Substitute.For<IDefinition>();
            definition.CanWriteIntoDirectoryOfUser.Returns(false);
            var outputFile = new OutputFile(Substitute.For<IOutputWriter>(), definition) as IOutputFile;
            
            // Act, When
            
            outputFile.WriteToFile(this.Fixture<string>());
            
            // Assert, Then

            System.IO.File.Exists(outputFile.Path).Should().BeFalse();
        }
        
        [Fact]
        public void os_métodos_de_escrita_devem_retornar_uma_auto_referência()
        {
            // Arrange, Given

            var outputFile = new OutputFile(
                Substitute.For<IOutputWriter>(),
                Program.DependencyResolver.GetInstance<IDefinition>()) as IOutputFile;
            
            // Act, When

            var retornoDeWrite = outputFile.Write(null);
            var retornoDeWriteLine = outputFile.WriteLine(null);
            
            // Assert, Then

            retornoDeWrite.Should().BeSameAs(outputFile);
            retornoDeWriteLine.Should().BeSameAs(outputFile);
        }
    }
}