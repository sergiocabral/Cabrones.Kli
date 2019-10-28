using System;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Output.File
{
    public class TestOutputFile: BaseForTest
    {
        public TestOutputFile()
        {
            DependencyResolverFromProgram.Register<IOutputFile, OutputFile>();
        }
        
        [Theory]
        [InlineData(typeof(OutputFile), 4)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(OutputFile), typeof(IOutput), typeof(IOutputFile))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

        [Fact]
        public void ao_escrever_um_texto_o_método_WriteToFile_deve_ser_chamado()
        {
            // Arrange, Given

            var outputWriter = Substitute.For<IOutputWriter>();
            var outputFile = new OutputFile(outputWriter, DependencyResolverFromProgram.GetInstance<IDefinition>()) as IOutputFile;
            var textoDeExemplo = Fixture.Create<string>();
            
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
                DependencyResolverFromProgram.GetInstance<IDefinition>()) as IOutputFile;
            var textoEscrito = Fixture.Create<string>();
            
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
            
            outputFile.WriteToFile(Fixture.Create<string>());
            
            // Assert, Then

            System.IO.File.Exists(outputFile.Path).Should().BeFalse();
        }
        
        [Fact]
        public void os_métodos_de_escrita_devem_retornar_uma_auto_referência()
        {
            // Arrange, Given

            var outputFile = new OutputFile(
                Substitute.For<IOutputWriter>(),
                DependencyResolverFromProgram.GetInstance<IDefinition>()) as IOutputFile;
            
            // Act, When

            var retornoDeWrite = outputFile.Write(null);
            var retornoDeWriteLine = outputFile.WriteLine(null);
            
            // Assert, Then

            retornoDeWrite.Should().BeSameAs(outputFile);
            retornoDeWriteLine.Should().BeSameAs(outputFile);
        }
    }
}